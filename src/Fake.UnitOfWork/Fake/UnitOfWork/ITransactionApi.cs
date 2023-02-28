using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fake.UnitOfWork;

public interface ITransactionApi : IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}