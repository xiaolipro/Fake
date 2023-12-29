using System.Linq.Expressions;
using Fake.DomainDrivenDesign.Entities;

namespace Fake.DomainDrivenDesign.Repositories;

/// <summary>
/// 仓储，专注于聚合的持久化
/// tips：仓储中的每一个行为都是最小执行单元
/// </summary>
public interface IRepository : IUnitOfWorkEnabled
{
}

/// <summary>
/// 仓储，专注于聚合的持久化
/// </summary>
/// <typeparam name="TEntity">聚合根</typeparam>
public interface IRepository<TEntity> : IRepository where TEntity : class, IAggregateRoot
{
    Task<IQueryable<TEntity>> GetQueryableAsync(
        bool isInclude = true,
        CancellationToken cancellationToken = default);

    Task<TEntity> FirstAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool isInclude = true,
        CancellationToken cancellationToken = default);

    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool isInclude = true,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Dictionary<string, bool>? sorting = null,
        bool isInclude = true,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetPaginatedListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        int pageIndex = 1,
        int pageSize = 20,
        Dictionary<string, bool>? sorting = null,
        bool isInclude = true,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task InsertRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    );
}