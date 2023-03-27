using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fake.Domain.Entities;
using Fake.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Domain.Repositories.EntityFrameWorkCore;

public class EfCoreRepository<TDbContext, TEntity> :RepositoryBase<TEntity>
    where TDbContext : DbContext 
    where TEntity : class, IAggregateRoot
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDbContextProvider<TDbContext> _dbContextProvider;

    protected EfCoreRepository(IServiceProvider serviceProvider):base(serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _dbContextProvider = serviceProvider.GetRequiredService<IDbContextProvider<TDbContext>>();
    }

    protected Task<TDbContext> GetDbContextAsync()
    {
        return _dbContextProvider.GetDbContextAsync();
    }

    public override async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var entry =await dbContext.Set<TEntity>().AddAsync(entity, GetCancellationToken(cancellationToken));

        if (autoSave) await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));

        return entry.Entity;
    }

    public override async Task InsertAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        
        await dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        
        if (autoSave) await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
    }

    public Task<TEntity> UpdateAsync(TEntity aggregate, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> UpdateAsync(IEnumerable<TEntity> aggregates, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TEntity aggregate, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(IEnumerable<TEntity> aggregates, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}