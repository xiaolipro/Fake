using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Fake.Domain.Entities;

namespace Fake.Domain.Repositories.EntityFrameWorkCore;

public interface IEfCoreRepository<TDbContext, TEntity>:IRepository<TEntity> where TEntity : class, IAggregateRoot
{
    Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default);
}

public interface IEfCoreRepository<TDbContext, TEntity, TKey> : IEfCoreRepository<TDbContext, TEntity>,IRepository<TEntity,TKey>
    where TEntity : class, IAggregateRoot<TKey>
{

}
