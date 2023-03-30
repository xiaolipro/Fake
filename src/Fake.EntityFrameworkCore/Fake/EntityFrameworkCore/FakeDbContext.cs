﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Fake.Domain.Entities;
using Fake.Domain.Entities.Auditing;
using Fake.Domain.Entities.IdGenerators;
using Fake.EntityFrameworkCore.Modeling;
using Fake.EntityFrameworkCore.ValueConverters;
using Fake.Reflection;
using Fake.Timing;
using Fake.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.EntityFrameworkCore;

public abstract class FakeDbContext<TDbContext> : DbContext where TDbContext : DbContext
{
    private readonly Lazy<IClock> _clock;
    private readonly Lazy<IGuidGenerator> _guidGenerator;
    private readonly Lazy<IAuditPropertySetter> _auditPropertySetter;

    private static readonly MethodInfo ConfigureGlobalFiltersMethodInfo = typeof(FakeDbContext<TDbContext>)
        .GetMethod(
            nameof(ConfigureGlobalFilters),
            BindingFlags.Instance | BindingFlags.NonPublic
        );

    protected FakeDbContext(DbContextOptions<TDbContext> options, IServiceProvider serviceProvider):base(options)
    {
        _clock = new Lazy<IClock>(serviceProvider.GetRequiredService<IClock>());
        _guidGenerator = new Lazy<IGuidGenerator>(serviceProvider.GetRequiredService<IGuidGenerator>());
        _auditPropertySetter =
            new Lazy<IAuditPropertySetter>(serviceProvider.GetRequiredService<IAuditPropertySetter>());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        TrySetDatabaseProvider(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            ConfigureBaseProperties(modelBuilder, entityType);
            ConfigureValueConverter(modelBuilder, entityType);
        }
    }

    public void Initialize(IUnitOfWork unitOfWork)
    {
        // 设置超时时间
        if (unitOfWork.Context.Timeout.HasValue && Database.IsRelational())
        {
            if (!Database.GetCommandTimeout().HasValue)
            {
                Database.SetCommandTimeout(TimeSpan.FromMilliseconds(unitOfWork.Context.Timeout.Value));
            }
        }

        // 级联删除策略
        ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;

        // 实体Track事件
        ChangeTracker.Tracked += (_, args) =>
        {
            // TODO：ExtraProperties Tracked?

            // 为跟踪实体发布事件
            PublishEventsForTrackedEntity(args.Entry);
        };

        // 实体状态变更事件
        ChangeTracker.StateChanged += (_, args) =>
        {
            // 为跟踪实体发布事件
            PublishEventsForTrackedEntity(args.Entry);
        };
    }

    protected virtual void PublishEventsForTrackedEntity(EntityEntry entry)
    {
        switch (entry.State)
        {
            case EntityState.Detached: //游离状态：上下文未跟踪该实体
                break;
            case EntityState.Unchanged: //未变更状态：该实体正在由上下文跟踪并存在于数据库中，其属性值与数据库中的值没有变化。
                break;
            case EntityState.Deleted: //删除状态：该实体正在由上下文跟踪并存在于数据库中，它已标记为从数据库中删除
                SoftDelete(entry);
                break;
            case EntityState.Modified: //修改状态：该实体正在由上下文跟踪并存在于数据库中，其部分或全部属性值已被修改
                SetModifier(entry);
                break;
            case EntityState.Added: //新增状态：上下文正在跟踪实体，但数据库中尚不存在该实体。
                CheckAndSetId(entry);
                SetVersionNum(entry);
                SetCreator(entry);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected virtual void SoftDelete(EntityEntry entry)
    {
        if (entry.Entity is ISoftDelete entityWithSoftDelete)
        {
            if (entityWithSoftDelete.HardDeleted) return;

            // 重置entity状态
            entry.Reload();

            ReflectionHelper.TrySetProperty(entityWithSoftDelete, x => x.IsDeleted, () => true);
            
            _auditPropertySetter.Value.SetModificationProperties(entry.Entity);
        }
    }

    protected virtual void SetModifier(EntityEntry entry)
    {
        // 只要有一个属性被修改了，且值不由数据库生成
        if (entry.Properties.Any(p => p.IsModified && p.Metadata.ValueGenerated == ValueGenerated.Never))
        {
            _auditPropertySetter.Value.SetModificationProperties(entry.Entity);
        }
    }

    protected virtual void SetCreator(EntityEntry entry)
    {
        _auditPropertySetter.Value.SetCreationProperties(entry.Entity);
        _auditPropertySetter.Value.SetModificationProperties(entry.Entity);
    }

    protected virtual void SetVersionNum(EntityEntry entry)
    {
        if (entry.Entity is IHasVersionNum entityWithVersionNum)
        {
            if (entityWithVersionNum.VersionNum != default) return;

            entityWithVersionNum.VersionNum = SimpleGuidGenerator.Instance.Create().ToString("N");
        }
    }

    protected virtual void CheckAndSetId(EntityEntry entry)
    {
        if (entry.Entity is IEntity<Guid> entityWithGuidId)
        {
            TrySetGuidId(entry, entityWithGuidId);
        }

        // TODO: 雪花id
    }

    protected virtual void TrySetGuidId(EntityEntry entry, IEntity<Guid> entityWithGuidId)
    {
        if (entityWithGuidId.Id != default) return;

        var idProperty = entry.Property(nameof(IEntity<Guid>.Id)).Metadata.PropertyInfo;

        var attr = ReflectionHelper.GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(idProperty);
        if (attr != null && attr.DatabaseGeneratedOption != DatabaseGeneratedOption.None) return;

        EntityHelper.TrySetId(entityWithGuidId, () => _guidGenerator.Value.Create(), true);
    }

    protected virtual void TrySetDatabaseProvider(ModelBuilder modelBuilder)
    {
        DatabaseProvider? provider = Database.ProviderName switch
        {
            "Microsoft.EntityFrameworkCore.SqlServer" => DatabaseProvider.SqlServer,
            "Pomelo.EntityFrameworkCore.MySql" => DatabaseProvider.MySql,
            _ => null
        };

        if (provider != null)
        {
            modelBuilder.SetDatabaseProvider(provider.Value);
        }
    }

    protected virtual void ConfigureBaseProperties(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
    {
        /*
         * 如果是附庸实体，意味着其作为一个字段直接由另一个实体持有，它没有自己的表。
         * 它会伴随所属实体一同被创建更新或销毁，在DDD中很生动的对应着值对象。
         */
        if (mutableEntityType.IsOwned()) return;

        if (!mutableEntityType.ClrType.IsAssignableTo(typeof(IEntity))) return;

        // TODO: 这里被迫选用一个类型事实上UserId类型是可以自定义的
        modelBuilder.Entity(mutableEntityType.ClrType)
            .TryConfigureCreator<Guid>()
            .TryConfigureModifier<Guid>()
            .TryConfigureSoftDelete<Guid>()
            .TryConfigureExtraProperties()
            .TryConfigureVersionNum();

        // 配置全局过滤器
        ConfigureGlobalFiltersMethodInfo
            .MakeGenericMethod(mutableEntityType.ClrType)
            .Invoke(this, new object[] { modelBuilder, mutableEntityType });
    }

    protected virtual void ConfigureValueConverter(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
    {
        if (mutableEntityType.BaseType != null) return;
        if (mutableEntityType.IsOwned()) return;

        var entityType = mutableEntityType.ClrType;
        if (entityType.IsDefined(typeof(OwnedAttribute), true)) return;

        if (!entityType.IsDefined(typeof(DisableDateTimeNormalizationAttribute)))
        {
            var dateTimeProperties = mutableEntityType.GetProperties()
                .Where(p =>
                {
                    var propertyInfo = p.PropertyInfo;
                    if (propertyInfo == null) return false;
                    if (!propertyInfo.CanWrite) return false;
                    if (ReflectionHelper.GetSingleAttributeOrDefault<DisableDateTimeNormalizationAttribute>(
                            propertyInfo, includeDeclaringType: true) != null) return false;
                    return propertyInfo.PropertyType == typeof(DateTime) ||
                           propertyInfo.PropertyType == typeof(DateTime?);
                });

            foreach (var property in dateTimeProperties)
            {
                modelBuilder.Entity(entityType).Property(property.Name)
                    .HasConversion(property.ClrType == typeof(DateTime)
                        ? new FakeDateTimeValueConverter(_clock.Value)
                        : new FakeNullableDateTimeValueConverter(_clock.Value));
            }
        }
    }

    protected virtual void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder,
        IMutableEntityType mutableEntityType)
        where TEntity : class
    {
        // 如果实体有父类则不应该拦截
        if (mutableEntityType.BaseType != null) return;

        Expression<Func<TEntity, bool>> expression = null;

        if (mutableEntityType.ClrType.IsAssignableTo(typeof(ISoftDelete)))
        {
            expression = entity => !EF.Property<bool>(entity, nameof(ISoftDelete.IsDeleted));
        }

        if (expression != null)
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(expression);
        }
    }
}