namespace Fake.Threading;

public interface ICancellationTokenProvider
{
    CancellationToken Token { get; }

    IDisposable Use(CancellationToken cancellationToken);
}