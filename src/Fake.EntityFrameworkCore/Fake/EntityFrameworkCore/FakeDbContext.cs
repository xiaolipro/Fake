using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Fake.DependencyInjection;
using Fake.Domain.Entities;
using Fake.Domain.Entities.Auditing;
using Fake.EntityFrameworkCore.Modeling;
using Fake.Timing;
using Fake.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fake.EntityFrameworkCore;

public class FakeDbContext<TDbContext> : DbContext, ITransientDependency where TDbContext : DbContext
{
    private readonly FakeDbContextOptions<TDbContext> _options;
    
    public IClock Clock;

    private static readonly MethodInfo ConfigureGlobalFiltersMethodInfo = typeof(FakeDbContext<TDbContext>)
        .GetMethod(
            nameof(ConfigureGlobalFilters),
            BindingFlags.Instance | BindingFlags.NonPublic
        );

    public FakeDbContext(FakeDbContextOptions<TDbContext> options) : base(options)
    {
        _options = options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            ConfigureBaseProperties(modelBuilder, entityType);
        }
    }
    public void Initialize(IUnitOfWork unitOfWork)
    {
        throw new System.NotImplementedException();
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