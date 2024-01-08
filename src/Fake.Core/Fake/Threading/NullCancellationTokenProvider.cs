namespace Fake.Threading;

public class NullCancellationTokenProvider()
    : CancellationTokenProviderBase(new AmbientScopeProvider<CancellationToken>())
{
    public static NullCancellationTokenProvider Instance { get; } = new();

    public override CancellationToken Token => CurrentToken;
}