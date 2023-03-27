using Fake.Domain.Entities;
using Fake.UnitOfWork;
using JetBrains.Annotations;

namespace Fake.Domain.Repositories;

public interface IRepository
{
    IUnitOfWork UnitOfWork { get; }
}

/// <summary>
/// 仓储，专注于聚合的持久化
/// </summary>
/// <typeparam name="TEntity">聚合根</typeparam>
public interface IRepository<TEntity> where TEntity : IAggregateRoot
{
    [NotNull]
    Task<TEntity> InsertAsync([NotNull] TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    [NotNull]
    Task InsertAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default);

    [NotNull]
    Task<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    [NotNull]
    Task UpdateAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteAsync([NotNull] TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false,
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
    [NotNull]
    Task<TEntity> InsertAsync(int id, bool autoSave = false,
        CancellationToken cancellationToken = default);

    [NotNull]
    Task<TEntity> InsertAsync([NotNull] IEnumerable<int> ids, bool autoSave = false,
        CancellationToken cancellationToken = default);

    [NotNull]
    Task<TEntity> UpdateAsync(int id, bool autoSave = false,
        CancellationToken cancellationToken = default);

    [NotNull]
    Task<TEntity> UpdateAsync([NotNull] IEnumerable<int> ids, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, bool autoSave = false, CancellationToken cancellationToken = default);

    Task DeleteAsync([NotNull] IEnumerable<int> ids, bool autoSave = false,
        CancellationToken cancellationToken = default);
}