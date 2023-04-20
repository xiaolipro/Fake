using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Fake.EntityFrameworkCore;

public interface IDbContextProvider<TDbContext> where TDbContext : DbContext
{
    Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default);
}