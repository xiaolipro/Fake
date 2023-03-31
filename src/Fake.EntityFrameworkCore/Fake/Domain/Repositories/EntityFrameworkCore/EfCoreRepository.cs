using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fake.Domain.Entities;
using Fake.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Domain.Repositories.EntityFrameWorkCore;

public class EfCoreRepository<TDbContext, TEntity> : RepositoryBase<TEntity>, IEfCoreRepository<TDbContext, TEntity>
    where TDbContext : DbContext
    where TEntity : class, IAggregateRoot
{
    private readonly IDbContextProvider<TDbContext> _dbContextProvider;

    public EfCoreRepository(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _dbContextProvider = serviceProvider.GetRequiredService<IDbContextProvider<TDbContext>>();
    }

    public Task<TDbContext> GetDbContextAsync()
    {
        return _dbContextProvider.GetDbContextAsync();
    }

    public async Task<DbSet<TEntity>> GetDbSetAsync()
    {
        var dbContext = await GetDbContextAsync();
        return dbContext.Set<TEntity>();
    }

    public override async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

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
        var dbContext = await GetDbContextAsync();

        await dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }
    }

    public override async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        
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

        var dbContext = await GetDbContextAsync();

        dbContext.Set<TEntity>().UpdateRange(entities);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public override async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

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

        var dbContext = await GetDbContextAsync();

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
    public EfCoreRepository(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public async Task<TEntity> GetFirstOrNullAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var set = await GetDbSetAsync();
        return await set.FindAsync(new object[]{id}, cancellationToken: GetCancellationToken(cancellationToken));
    }

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
        cancellationToken = GetCancellationToken(cancellationToken);

        var set = await GetDbSetAsync();
        var entities = await set.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

        await DeleteRangeAsync(entities, autoSave, cancellationToken);
    }
}