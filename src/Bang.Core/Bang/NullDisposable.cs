namespace Bang.Core;

public sealed class NullDisposable : IDisposable
{
    public static NullDisposable Instance { get; } = new();
    public void Dispose()
    {
    }
}