using Fake.DependencyInjection;
using Fake.Domain.Entities;
using Fake.Threading;
using Fake.UnitOfWork;

namespace Fake.Domain.Repositories;

public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
    where TEntity : class, IAggregateRoot
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public IFakeLazyServiceProvider LazyServiceProvider { get; set;}  // 属性注入
    
    public ICancellationTokenProvider CancellationTokenProvider=> LazyServiceProvider.GetRequiredLazyService<ICancellationTokenProvider>();
    public IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.GetRequiredLazyService<IUnitOfWorkManager>();

    protected virtual CancellationToken GetCancellationToken(CancellationToken preferredValue = default)
    {
        return CancellationTokenProvider.FallbackToProvider(preferredValue);
    }

    protected virtual Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return UnitOfWorkManager.Current != null
            ? UnitOfWorkManager.Current.SaveChangesAsync(cancellationToken)
            : Task.CompletedTask;
    }

    public abstract Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    public virtual async Task InsertRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await InsertAsync(entity, cancellationToken: cancellationToken);
        }
        if (autoSave) await SaveChangesAsync(cancellationToken);
    }

    public abstract Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);
    public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await UpdateAsync(entity, cancellationToken: cancellationToken);
        }
        if (autoSave) await SaveChangesAsync(cancellationToken);
    }

    public abstract Task DeleteAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await DeleteAsync(entity, cancellationToken: cancellationToken);
        }
        if (autoSave) await SaveChangesAsync(cancellationToken);
    }
}

public abstract class RepositoryBase<TEntity,TKey> :  RepositoryBase<TEntity>, IRepository<TEntity,TKey>
    where TEntity : class, IAggregateRoot<TKey>
{
    public abstract Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);

    public async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entity = await GetAsync(id, cancellationToken);
        if (entity == null)
        {
            return;
        }

        await DeleteAsync(entity, autoSave, cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        foreach (var id in ids)
        {
            await DeleteAsync(id, cancellationToken:cancellationToken);
        }

        if (autoSave)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }
}