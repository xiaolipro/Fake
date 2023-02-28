using System.Threading;
using System.Threading.Tasks;

namespace Fake.UnitOfWork;

public interface ISupportSaveChanges
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}