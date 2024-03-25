using System.Linq.Expressions;
using System.Threading;
using Fake.SqlSugarCore;

namespace Fake.DomainDrivenDesign.Repositories.SqlSugarCore;

public class SqlSugarRepository<TDbContext, TEntity> : RepositoryBase<TEntity>, ISqlSugarRepository<TDbContext, TEntity>
    where TDbContext : SugarDbContext
    where TEntity : class, IAggregateRoot, new()
{
    public async Task<TDbContext> GetDbContextAsync()
    {
        return await LazyServiceProvider.GetRequiredService<ISugarDbContextProvider<TDbContext>>().GetDbContextAsync();
    }

    public override Task<IQueryable<TEntity>> GetQueryableAsync(bool isInclude = true,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null,
        bool isInclude = true,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Dictionary<string, bool>? sorting = null, bool isInclude = true,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<List<TEntity>> GetPaginatedListAsync(Expression<Func<TEntity, bool>>? predicate,
        int pageIndex = 1, int pageSize = 20, Dictionary<string, bool>? sorting = null,
        bool isInclude = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<long> GetCountAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task DeleteAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}