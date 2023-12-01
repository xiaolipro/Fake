namespace Fake;

public class DisposableWrapper : IDisposable
{
    private readonly Action _action;

    public DisposableWrapper(Action action)
    {
        ThrowHelper.ThrowIfNull(action, nameof(action));

        _action = action;
    }

    public void Dispose()
    {
        _action.Invoke();
    }
}