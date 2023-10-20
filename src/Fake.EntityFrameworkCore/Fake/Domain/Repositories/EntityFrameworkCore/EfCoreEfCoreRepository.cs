using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Fake.Domain.Entities;
using Fake.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fake.Domain.Repositories.EntityFrameWorkCore;

public class EfCoreEfCoreRepository<TDbContext, TEntity> : RepositoryBase<TEntity>,
    IEfCoreRepository<TDbContext, TEntity>
    where TDbContext : FakeDbContext<TDbContext>
    where TEntity : class, IAggregateRoot
{
    private IDbContextProvider<TDbContext> DbContextProvider =>
        LazyServiceProvider.GetRequiredLazyService<IDbContextProvider<TDbContext>>();

    public Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
    {
        return DbContextProvider.GetDbContextAsync(cancellationToken);
    }

    public async Task<DbSet<TEntity>> GetDbSetAsync(CancellationToken cancellationToken = default)
    {
        return (await GetDbContextAsync(cancellationToken)).Set<TEntity>();
    }

    public override async Task<IQueryable<TEntity>> GetQueryableAsync(
        bool isInclude = true,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var dbSet = await GetDbSetAsync(cancellationToken);

        var query = dbSet.AsQueryable();
        if (isInclude)
        {
            var entityType = dbSet.EntityType.Model.FindEntityType(typeof(TEntity));

            if (entityType != null)
            {
                query = entityType.GetNavigations().Aggregate(
                    query,
                    (current, navigationProperty)
                        => current.Include(navigationProperty.Name)
                );
            }
        }

        return query;
    }

    public override async Task<TEntity> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        bool isInclude = true,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var query = await GetQueryableAsync(isInclude, cancellationToken);

        return await query.WhereIf(predicate != null, predicate).FirstOrDefaultAsync(cancellationToken);
    }

    public override async Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Dictionary<string, bool> sorting = null,
        bool isInclude = true,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var query = await GetQueryableAsync(isInclude, cancellationToken);

        sorting ??= new Dictionary<string, bool>();
        return await query.WhereIf(predicate != null, predicate).OrderBy(sorting).ToListAsync(cancellationToken);
    }

    public override async Task<List<TEntity>> GetPaginatedListAsync(
        Expression<Func<TEntity, bool>> predicate,
        int pageIndex = 1,
        int pageSize = 20,
        Dictionary<string, bool> sorting = null,
        bool isInclude = true,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var query = await GetQueryableAsync(isInclude, cancellationToken);

        return await query
            .WhereIf(predicate != null, predicate)
            .OrderBy(sorting)
            .Skip(((pageIndex < 1 ? 1 : pageIndex) - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public override async Task<long> GetCountAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var query = await GetQueryableAsync(false, cancellationToken);

        return await query.WhereIf(predicate != null, predicate).LongCountAsync(cancellationToken);
    }

    public override async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var query = await GetQueryableAsync(false, cancellationToken);

        return await query.WhereIf(predicate != null, predicate).AnyAsync(cancellationToken);
    }

    public override async Task<TEntity> InsertAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync(cancellationToken);

        /*
         * EFCore的AddAsync与Add区别
         * https://stackoverflow.com/questions/47135262/addasync-vs-add-in-ef-core
         */
        var entry = dbContext.Add(entity);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }

        return entry.Entity;
    }

    public override async Task InsertRangeAsync(
        IEnumerable<TEntity> entities,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync(cancellationToken);

        await dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }
    }

    public override async Task<TEntity> UpdateAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync(cancellationToken);

        dbContext.Attach(entity);
        var entry = dbContext.Update(entity);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }

        return entry.Entity;
    }

    public override async Task UpdateRangeAsync(
        IEnumerable<TEntity> entities,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var dbContext = await GetDbContextAsync(cancellationToken);

        dbContext.Set<TEntity>().UpdateRange(entities);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public override async Task DeleteAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync(cancellationToken);

        dbContext.Set<TEntity>().Remove(entity);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }
    }

    public override async Task DeleteRangeAsync(
        IEnumerable<TEntity> entities,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var dbContext = await GetDbContextAsync(cancellationToken);

        dbContext.RemoveRange(entities);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public override async Task DeleteAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        var dbContext = await GetDbContextAsync(cancellationToken);

        var entities = await dbContext.Set<TEntity>()
            .Where(predicate)
            .ToListAsync(cancellationToken);

        await DeleteRangeAsync(entities, autoSave, cancellationToken);
    }
}