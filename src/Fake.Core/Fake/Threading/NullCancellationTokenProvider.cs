namespace Fake.Threading;

public class NullCancellationTokenProvider:CancellationTokenProviderBase
{
    public static NullCancellationTokenProvider Instance { get; } = new();
    
    public NullCancellationTokenProvider() : base(new AmbientScopeProvider<CancellationToken>())
    {
    }

    public override CancellationToken Token => CurrentToken;
}