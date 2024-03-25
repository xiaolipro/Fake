using Fake.SqlSugarCore;

namespace Fake.DomainDrivenDesign.Repositories.SqlSugarCore;

public interface ISqlSugarRepository<TDbContext, TEntity> : IRepository<TEntity>
    where TDbContext : SugarDbContext
    where TEntity : class, IAggregateRoot
{
    Task<TDbContext> GetDbContextAsync();
}