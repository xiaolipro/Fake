using System.Threading.Tasks;
using Fake.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fake.Domain.Repositories.EntityFrameWorkCore;

public interface IEfCoreRepository<TDbContext, TEntity>:IRepository<TEntity> where TEntity : class, IAggregateRoot
{
    Task<TDbContext> GetDbContextAsync();

    Task<DbSet<TEntity>> GetDbSetAsync();
}

public interface IEfCoreRepository<TDbContext, TEntity, TKey> : IEfCoreRepository<TDbContext, TEntity>,IRepository<TEntity,TKey>
    where TEntity : class, IAggregateRoot<TKey>
{

}
