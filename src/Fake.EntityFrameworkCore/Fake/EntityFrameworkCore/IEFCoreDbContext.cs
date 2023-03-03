using System.Threading;
using System.Threading.Tasks;

namespace Fake.EntityFrameworkCore;

public interface IEFCoreDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}