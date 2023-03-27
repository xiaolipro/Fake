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

    public virtual async Task InsertAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await InsertAsync(entity, cancellationToken: cancellationToken);
        }
        if (autoSave) await SaveChangesAsync(cancellationToken);
    }

    public Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}