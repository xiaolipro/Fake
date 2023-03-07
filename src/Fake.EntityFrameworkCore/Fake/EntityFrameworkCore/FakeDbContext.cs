using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using Fake.DependencyInjection;
using Fake.Domain.Entities;
using Fake.Domain.Entities.Auditing;
using Fake.EntityFrameworkCore.Modeling;
using Fake.Reflection;
using Fake.Timing;
using Fake.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fake.EntityFrameworkCore;

public class FakeDbContext<TDbContext> : DbContext, ITransientDependency where TDbContext : DbContext
{
    private readonly Lazy<IFakeClock> _clock;
    private readonly EfCoreOptions _options;
    
    private static readonly MethodInfo ConfigureGlobalFiltersMethodInfo = typeof(FakeDbContext<TDbContext>)
        .GetMethod(
            nameof(ConfigureGlobalFilters),
            BindingFlags.Instance | BindingFlags.NonPublic
        );

    public FakeDbContext(IServiceProvider serviceProvider, IOptions<EfCoreOptions> options)
    {
        _options = serviceProvider.GetRequiredService<IOptions<EfCoreOptions>>().Value;
        _clock = new Lazy<IFakeClock>(serviceProvider.GetRequiredService<IFakeClock>);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        TrySetDatabaseProvider(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            ConfigureBaseProperties(modelBuilder, entityType);
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
        ChangeTracker.Tracked += (sender, args) =>
        {
            // TODO：ExtraProperties Tracked?
            
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
                break;
            case EntityState.Modified: //修改状态：该实体正在由上下文跟踪并存在于数据库中，其部分或全部属性值已被修改
                break;
            case EntityState.Added: //新增状态：上下文正在跟踪实体，但数据库中尚不存在该实体。
                CheckAndSetId(entry);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected virtual void CheckAndSetId(EntityEntry entry)
    {
        if (entry.Entity is IEntity<Guid> entityWithGuidId)
        {
            TrySetGuidId(entry, entityWithGuidId);
        }
    }

    protected virtual void TrySetGuidId(EntityEntry entry, IEntity<Guid> entityWithGuidId)
    {
        if (entityWithGuidId.Id != default) return;
        
        var idProperty = entry.Property(nameof(IEntity<Guid>.Id)).Metadata.PropertyInfo;

        var attr = ReflectionHelper.GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(idProperty);
        if (attr != null && attr.DatabaseGeneratedOption != DatabaseGeneratedOption.None) return;
        
        EntityHelper.TrySetId(entry, default, true);
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
        // 可移动实体类型是以特殊方式持久存储的实体，其有一个外键关联到另一个实体，而没有它自己的表。
        // 可移动的实例会与主要实例一起被创建和保存，而不仅仅是单独的外键。
        if (mutableEntityType.IsOwned()) return;

        if (!mutableEntityType.ClrType.IsAssignableTo(typeof(IEntity))) return;

        // TODO: 这里被迫选用一个类型事实上UserId类型是可以自定义的
        modelBuilder.Entity(mutableEntityType.ClrType)
            .TryConfigureCreator<Guid>()
            .TryConfigureModifier<Guid>()
            .TryConfigureDeleter<Guid>()
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
    }

    protected virtual void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder,
        IMutableEntityType mutableEntityType)
        where TEntity : class
    {
        // TODO: 我没看懂这个设计
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