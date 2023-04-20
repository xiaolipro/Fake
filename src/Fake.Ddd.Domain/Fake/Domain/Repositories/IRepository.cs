using Fake.Domain.Entities;
using Fake.UnitOfWork;
using JetBrains.Annotations;

namespace Fake.Domain.Repositories;

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
public interface IRepository<TEntity> : IRepository where TEntity : IAggregateRoot
{
    [NotNull]
    Task<TEntity> InsertAsync([NotNull] TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    [NotNull]
    Task InsertRangeAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default);

    [NotNull]
    Task<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    [NotNull]
    Task UpdateRangeAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteAsync([NotNull] TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteRangeAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// 仓储，关注于单一聚合的持久化
/// </summary>
/// <typeparam name="TEntity">聚合根</typeparam>
/// <typeparam name="TKey">主键类型</typeparam>
// ReSharper disable once TypeParameterCanBeVariant
public interface IRepository<TEntity, TKey> : IRepository<TEntity>
    where TEntity : IAggregateRoot<TKey>
{
    Task<TEntity> GetFirstOrNullAsync(TKey id, CancellationToken cancellationToken = default);

    Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync([NotNull] IEnumerable<TKey> ids, bool autoSave = false,
        CancellationToken cancellationToken = default);
}