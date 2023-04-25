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

public class EfCoreRepository<TDbContext, TEntity> : RepositoryBase<TEntity>, IEfCoreRepository<TDbContext, TEntity>
    where TDbContext : DbContext
    where TEntity : class, IAggregateRoot
{
    private IDbContextProvider<TDbContext> DbContextProvider =>
        ServiceProvider.GetRequiredLazyService<IDbContextProvider<TDbContext>>();

    public Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
    {
        return DbContextProvider.GetDbContextAsync(cancellationToken);
    }

    public async Task<DbSet<TEntity>> GetDbSetAsync(CancellationToken cancellationToken = default)
    {
        return (await GetDbContextAsync(cancellationToken)).Set<TEntity>();
    }

    public override async Task<IQueryable<TEntity>> GetQueryableAsync(bool isInclude = false,
        CancellationToken cancellationToken = default)
    {
        if (isInclude)
        {
            var dbContext = (await GetDbContextAsync(cancellationToken));
            var query = dbContext.Set<TEntity>().AsQueryable();
            var entityType = dbContext.Model.FindEntityType(typeof(TEntity));

            if (entityType == null) return query;

            // 获取导航属性
            var navigationProperties = entityType.GetNavigations();
            
            return navigationProperties.Aggregate(query,
                (current, navigationProperty) => current.Include(navigationProperty.Name));
        }

        return (await GetDbSetAsync(cancellationToken)).AsQueryable();
    }

    public override async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression,
        bool isInclude = false, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var query = await GetQueryableAsync(isInclude, cancellationToken);

        if (expression != null)
        {
            query = query.Where(expression);
        }
        
        return await query.ToListAsync(cancellationToken);
    }

    public override async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync(cancellationToken);

        /*
         * EFCore的AddAsync与Add区别
         * https://stackoverflow.com/questions/47135262/addasync-vs-add-in-ef-core
         */
        var entry = await dbContext.AddAsync(entity, GetCancellationToken(cancellationToken));

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }

        return entry.Entity;
    }

    public override async Task InsertRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync(cancellationToken);

        await dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }
    }

    public override async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync(cancellationToken);

        dbContext.Attach(entity);
        var entry = dbContext.Set<TEntity>().Update(entity);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }

        return entry.Entity;
    }

    public override async Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false,
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

    public override async Task DeleteAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync(cancellationToken);

        dbContext.Set<TEntity>().Remove(entity);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }
    }

    public override async Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false,
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
}

public class EfCoreRepository<TDbContext, TEntity, TKey> : EfCoreRepository<TDbContext, TEntity>,
    IEfCoreRepository<TDbContext, TEntity, TKey>
    where TDbContext : DbContext
    where TEntity : class, IAggregateRoot<TKey>
{
    public async Task<TEntity> GetAsync(TKey id, bool isInclude = false, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var query = await GetQueryableAsync(isInclude, cancellationToken);

        return await query.FirstOrDefaultAsync(x => x.Id.Equals(id),
            cancellationToken);
    }

    public async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entity = await GetAsync(id, true, cancellationToken);
        if (entity == null)
        {
            return;
        }

        await DeleteAsync(entity, autoSave, cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<TKey> ids, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var db = await GetDbContextAsync(cancellationToken);
        var entities = await db.Set<TEntity>().Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

        await DeleteRangeAsync(entities, autoSave, cancellationToken);
    }
}