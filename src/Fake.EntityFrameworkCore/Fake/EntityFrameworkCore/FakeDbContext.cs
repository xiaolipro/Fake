using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Fake.Data;
using Fake.DependencyInjection;
using Fake.Domain;
using Fake.Domain.Entities;
using Fake.Domain.Entities.Auditing;
using Fake.EntityFrameworkCore.Auditing;
using Fake.EntityFrameworkCore.Modeling;
using Fake.EntityFrameworkCore.ValueConverters;
using Fake.EventBus;
using Fake.Helpers;
using Fake.IdGenerators;
using Fake.IdGenerators.GuidGenerator;
using Fake.Timing;
using Fake.UnitOfWork;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fake.EntityFrameworkCore;

public abstract class FakeDbContext<TDbContext> : DbContext where TDbContext : DbContext
{
    public ILazyServiceProvider ServiceProvider { get; [UsedImplicitly] set; }

    private static readonly MethodInfo ConfigureBasePropertiesMethodInfo = typeof(FakeDbContext<TDbContext>)
        .GetMethod(
            nameof(ConfigureBaseProperties),
            BindingFlags.Instance | BindingFlags.NonPublic
        );

    protected FakeDbContext(DbContextOptions<TDbContext> options) : base(options)
    {
    }

    private IFakeClock FakeClock => ServiceProvider.GetRequiredLazyService<IFakeClock>();
    private GuidGeneratorBase GuidGenerator => ServiceProvider.GetRequiredLazyService<GuidGeneratorBase>();
    private LongIdGeneratorBase LongIdGenerator => ServiceProvider.GetRequiredLazyService<LongIdGeneratorBase>();
    private IEventPublisher EventPublisher => ServiceProvider.GetRequiredLazyService<IEventPublisher>();
    private IAuditPropertySetter AuditPropertySetter => ServiceProvider.GetRequiredLazyService<IAuditPropertySetter>();
    private IEntityChangeHelper EntityChangeHelper => ServiceProvider.GetRequiredLazyService<IEntityChangeHelper>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        TrySetDatabaseProvider(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            entityType.AddIgnored(nameof(IEntity.DomainEvents));
            ConfigureBasePropertiesMethodInfo
                .MakeGenericMethod(entityType.ClrType)
                .Invoke(this, new object[] { modelBuilder, entityType });

            ConfigureValueConverter(modelBuilder, entityType);
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var changes = EntityChangeHelper.CreateChangeList(ChangeTracker.Entries());

            await BeforeSaveChangesAsync();
            var res = await base.SaveChangesAsync(cancellationToken);
            PublishDomainEvents();

            // id, time ..
            EntityChangeHelper.UpdateChangeList(changes);
            return res;
        }
        catch (DbUpdateConcurrencyException? ex)
        {
            throw new FakeDbConcurrencyException(ex.Message, ex);
        }
        finally
        {
            ChangeTracker.AutoDetectChangesEnabled = true;
        }
    }

    private void PublishDomainEvents()
    {
        var domainEntities = base.ChangeTracker.Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .OrderBy(x => x.Order)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        domainEvents.ForEach(@event => EventPublisher.Publish(@event));
    }

    protected virtual Task BeforeSaveChangesAsync()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            // Deleted状态可能是软删，也走的更新，同受版本约束
            if (entry.State.IsIn(EntityState.Modified, EntityState.Deleted))
            {
                if (entry.Entity is IHasVersionNum entity)
                {
                    // 保存更改时，将原始值与当前值比较，以确认是否需要更新
                    Entry(entity).Property(x => x.VersionNum).OriginalValue = entity.VersionNum;
                    entity.VersionNum = SimpleGuidGenerator.Instance.GenerateAsString();
                }
            }
        }

        return Task.CompletedTask;
    }

    public virtual void Initialize(IUnitOfWork unitOfWork)
    {
        // 设置超时时间
        if (Database.IsRelational())
        {
            if (!Database.GetCommandTimeout().HasValue)
            {
                Database.SetCommandTimeout(TimeSpan.FromMilliseconds(unitOfWork.Context.Timeout));
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

            // todo: abp在这里重置entity状态，但是重置状态会重新加载数据，为什么要这么做？
            //entry.Reload();

            ReflectionHelper.TrySetProperty(entityWithSoftDelete, x => x.IsDeleted, () => true);
            AuditPropertySetter.SetModificationProperties(entry.Entity);
        }
    }

    protected virtual void SetModifier(EntityEntry entry)
    {
        // 只要有一个属性被修改了，且值不由数据库生成
        if (entry.Properties.Any(p => p.IsModified && p.Metadata.ValueGenerated == ValueGenerated.Never))
        {
            AuditPropertySetter.SetModificationProperties(entry.Entity);
        }
    }

    protected virtual void SetCreator(EntityEntry entry)
    {
        AuditPropertySetter.SetCreationProperties(entry.Entity);
        AuditPropertySetter.SetModificationProperties(entry.Entity);
    }

    protected virtual void SetVersionNum(EntityEntry entry)
    {
        if (entry.Entity is IHasVersionNum entityWithVersionNum)
        {
            if (entityWithVersionNum.VersionNum != default) return;

            entityWithVersionNum.VersionNum = SimpleGuidGenerator.Instance.Generate().ToString("N");
        }
    }

    protected virtual void CheckAndSetId(EntityEntry entry)
    {
        if (!ShouldSetId(entry)) return;

        if (entry.Entity is IEntity<Guid> entityWithGuidId)
        {
            EntityHelper.TrySetId(entityWithGuidId, () => GuidGenerator.Generate(), true);
        }

        if (entry.Entity is IEntity<long> entityWithLongId)
        {
            EntityHelper.TrySetId(entityWithLongId, () => LongIdGenerator.Generate(), true);
        }
    }


    protected virtual bool ShouldSetId(EntityEntry entry)
    {
        if (entry.Entity is not Entity entity) return false;

        if (!entity.IsTransient) return false;

        var idProperty = entry.Property(nameof(IEntity<Guid>.Id)).Metadata.PropertyInfo;

        var attr = ReflectionHelper.GetAttributeOrDefault<DatabaseGeneratedAttribute>(idProperty);
        return attr == null || attr.DatabaseGeneratedOption == DatabaseGeneratedOption.None;
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

    protected virtual void ConfigureBaseProperties<TEntity>(ModelBuilder modelBuilder,
        IMutableEntityType mutableEntityType) where TEntity : class
    {
        /*
         * 如果是附庸实体，意味着其作为一个字段直接由另一个实体持有，它没有自己的表。
         * 它会伴随所属实体一同被创建更新或销毁，在DDD中很生动的对应着值对象。
         */
        if (mutableEntityType.IsOwned()) return;

        if (!typeof(TEntity).IsAssignableTo(typeof(IEntity))) return;

        // TODO: 这里被迫选用一个用户类型事实上UserId类型是可以自定义的
        modelBuilder.Entity<TEntity>()
            .TryConfigureCreator<Guid>()
            .TryConfigureModifier<Guid>()
            .TryConfigureSoftDelete()
            .TryConfigureExtraProperties()
            .TryConfigureVersionNum();

        // 配置全局过滤器
        ConfigureGlobalFilters<TEntity>(modelBuilder, mutableEntityType);
    }

    protected virtual void ConfigureValueConverter(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
    {
        if (mutableEntityType.BaseType != null) return;
        if (mutableEntityType.IsOwned()) return;

        var entityType = mutableEntityType.ClrType;
        if (entityType.IsDefined(typeof(OwnedAttribute), true)) return;

        var properties = mutableEntityType.GetProperties().ToList();

        foreach (var property in properties.Where(p =>
                     p.PropertyInfo?.PropertyType.IsAssignableTo<Enumeration>() ?? false))
        {
            modelBuilder.Entity(entityType).Property(property.Name).HasConversion(new FakeEnumerationValueConverter());
        }

        if (!entityType.IsDefined(typeof(DisableClockNormalizationAttribute)))
        {
            var dateTimeProperties = properties
                .Where(p =>
                {
                    var propertyInfo = p.PropertyInfo;
                    if (propertyInfo == null) return false;
                    if (!propertyInfo.CanWrite) return false;
                    if (ReflectionHelper.GetAttributeOrDefault<DisableClockNormalizationAttribute>(
                            propertyInfo, includeDeclaringType: true) != null) return false;
                    return propertyInfo.PropertyType == typeof(DateTime) ||
                           propertyInfo.PropertyType == typeof(DateTime?);
                });

            foreach (var property in dateTimeProperties)
            {
                modelBuilder.Entity(entityType).Property(property.Name)
                    .HasConversion(property.ClrType == typeof(DateTime)
                        ? new FakeDateTimeValueConverter(FakeClock)
                        : new FakeNullableDateTimeValueConverter(FakeClock));
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

        if (typeof(TEntity).IsAssignableTo(typeof(ISoftDelete)))
        {
            expression = entity => !EF.Property<bool>(entity, nameof(ISoftDelete.IsDeleted));
        }

        if (expression != null)
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(expression);
        }
    }
}