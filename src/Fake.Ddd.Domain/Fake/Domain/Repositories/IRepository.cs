using System.Linq.Expressions;
using Fake.Domain.Entities;
using JetBrains.Annotations;

namespace Fake.Domain.Repositories;

/// <summary>
/// 仓储，专注于聚合的持久化
/// tips：仓储中的每一个行为都是最小执行单元
/// </summary>
public interface IRepository: IUnitOfWorkEnabled
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

    Task<TEntity> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        bool isInclude = true,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Dictionary<string, bool> sorting = null,
        bool isInclude = true,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetPaginatedListAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        int pageIndex = 1,
        int pageSize = 20,
        Dictionary<string, bool> sorting = null,
        bool isInclude = true,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null,
        CancellationToken cancellationToken = default);

    [NotNull]
    Task<TEntity> InsertAsync([NotNull] TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task InsertRangeAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default);

    [NotNull]
    Task<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task UpdateRangeAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteAsync([NotNull] TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteRangeAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        [NotNull] Expression<Func<TEntity, bool>> predicate,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    );
}