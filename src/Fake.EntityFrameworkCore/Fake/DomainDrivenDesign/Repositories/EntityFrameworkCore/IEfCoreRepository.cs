namespace Fake.DomainDrivenDesign.Repositories.EntityFrameWorkCore;

public interface IEfCoreRepository<TDbContext, TEntity> : IRepository<TEntity>
    where TEntity : class, IAggregateRoot
{
    Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default);

    Task<DbSet<TEntity>> GetDbSetAsync(CancellationToken cancellationToken = default);
}