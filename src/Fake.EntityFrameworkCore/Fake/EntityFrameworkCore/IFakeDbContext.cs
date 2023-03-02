using System.Threading;
using System.Threading.Tasks;
using Fake.DependencyInjection;
using Fake.Domain.Entities;
using Fake.EntityFrameworkCore.Modeling;
using Fake.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fake.EntityFrameworkCore;

public interface IFakeDbContext
{
    void Initialize(IUnitOfWork unitOfWork);
}

public class FakeDbContext<TDbContext> : DbContext, IFakeDbContext, ITransientDependency where TDbContext : DbContext
{
    public FakeDbContext(DbContextOptions<TDbContext> options): base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            
        }
    }
    
    

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    public void Initialize(IUnitOfWork unitOfWork)
    {
        throw new System.NotImplementedException();
    }
    
    
    protected virtual void ConfigureBaseProperties(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
    {
        if (mutableEntityType.IsOwned())
        {
            return;
        }

        mutableEntityType.FindPrimaryKey()
            
        if (!mutableEntityType.ClrType.IsAssignableTo(typeof(IEntity)))
        {
            return;
        }

        modelBuilder.Entity(mutableEntityType.ClrType)
            .TryConfigurePrimaryKey<>();

        ConfigureGlobalFilters<TEntity>(modelBuilder, mutableEntityType);
    }
}