using System.Threading;
using System.Threading.Tasks;

namespace Fake.EntityFrameworkCore;

public interface IEfCoreDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}