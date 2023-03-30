using Fake.DependencyInjection;
using Fake.Domain.Entities;
using Fake.Threading;
using Fake.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Domain.Repositories;

public abstract class RepositoryBase<TEntity> : IRepository<TEntity>, IServiceProviderAccessor, IUnitOfWorkEnabled
    where TEntity : class, IAggregateRoot
{
    public readonly Lazy<ICancellationTokenProvider> CancellationTokenProvider;
    public readonly Lazy<IUnitOfWorkManager> UnitOfWorkManager;
    public IServiceProvider ServiceProvider { get; }

    protected RepositoryBase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        CancellationTokenProvider =
            new Lazy<ICancellationTokenProvider>(serviceProvider.GetRequiredService<ICancellationTokenProvider>);
        UnitOfWorkManager = new Lazy<IUnitOfWorkManager>(serviceProvider.GetRequiredService<IUnitOfWorkManager>);
    }

    protected virtual CancellationToken GetCancellationToken(CancellationToken preferredValue = default)
    {
        return CancellationTokenProvider.Value.FallbackToProvider(preferredValue);
    }

    protected virtual Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return UnitOfWorkManager.Value.Current != null
            ? UnitOfWorkManager.Value.Current.SaveChangesAsync(cancellationToken)
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
    public virtual Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public abstract Task DeleteAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    public virtual Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

public abstract class RepositoryBase<TEntity,TKey> :  RepositoryBase<TEntity>, IRepository<TEntity,TKey>
    where TEntity : class, IAggregateRoot<TKey>
{
    protected RepositoryBase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
    public abstract Task<TEntity> GetFirstOrNullAsync(TKey id, CancellationToken cancellationToken = default);

    public async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entity = await GetFirstOrNullAsync(id, cancellationToken);
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