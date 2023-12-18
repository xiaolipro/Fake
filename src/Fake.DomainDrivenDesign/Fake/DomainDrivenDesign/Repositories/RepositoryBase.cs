using System.Linq.Expressions;
using Fake.DependencyInjection;
using Fake.DomainDrivenDesign.Entities;
using Fake.Threading;

namespace Fake.DomainDrivenDesign.Repositories;

/// <summary>
/// 仓储基类
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
    where TEntity : class, IAggregateRoot
{
    public ILazyServiceProvider LazyServiceProvider { get; set; } = null!; // 属性注入

    public ICancellationTokenProvider CancellationTokenProvider =>
        LazyServiceProvider.GetRequiredLazyService<ICancellationTokenProvider>();

    public IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.GetRequiredLazyService<IUnitOfWorkManager>();
    public IUnitOfWork? UnitOfWork => UnitOfWorkManager.Current;

    protected virtual CancellationToken GetCancellationToken(CancellationToken preferredValue = default)
    {
        return CancellationTokenProvider.FallbackToProvider(preferredValue);
    }

    protected virtual Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return UnitOfWork != null
            ? UnitOfWork.SaveChangesAsync(cancellationToken)
            : Task.CompletedTask;
    }

    public abstract Task<IQueryable<TEntity>> GetQueryableAsync(
        bool isInclude = true,
        CancellationToken cancellationToken = default);

    public abstract Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool isInclude = true,
        CancellationToken cancellationToken = default);

    public abstract Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Dictionary<string, bool>? sorting = null,
        bool isInclude = true,
        CancellationToken cancellationToken = default);

    public abstract Task<List<TEntity>> GetPaginatedListAsync(
        Expression<Func<TEntity, bool>>? predicate,
        int pageIndex = 1,
        int pageSize = 20,
        Dictionary<string, bool>? sorting = null,
        bool isInclude = true,
        CancellationToken cancellationToken = default);

    public abstract Task<long> GetCountAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    public abstract Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

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

    public abstract Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false,
        CancellationToken cancellationToken = default);
}