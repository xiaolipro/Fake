using System.Collections.Generic;
using System.Linq;
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
        LazyServiceProvider.GetRequiredLazyService<IDbContextProvider<TDbContext>>();

    public Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
    {
        return DbContextProvider.GetDbContextAsync(cancellationToken);
    }

    public override async Task<IQueryable<TEntity>> GetQueryableAsync(CancellationToken cancellationToken = default)
    {
        return (await GetDbContextAsync(cancellationToken)).Set<TEntity>().AsQueryable();
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
    public async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var db = await GetDbContextAsync(cancellationToken);
        return await db.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id),
            cancellationToken: GetCancellationToken(cancellationToken));
    }

    public async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entity = await GetAsync(id, cancellationToken);
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