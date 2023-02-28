using System.Threading;
using System.Threading.Tasks;

namespace Fake.UnitOfWork;

public interface ISupportRollback
{
    Task RollbackAsync(CancellationToken cancellationToken = default);
}