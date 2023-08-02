using System.Threading;
using System.Threading.Tasks;
using Fake.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fake.Domain.Repositories.EntityFrameWorkCore;

public interface IEfCoreRepository<TDbContext, TEntity> : IRepository<TEntity>
    where TEntity : class, IAggregateRoot
{
    Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default);

    Task<DbSet<TEntity>> GetDbSetAsync(CancellationToken cancellationToken = default);
}