namespace Bang;

public class DisposableWrapper:IDisposable
{
    private readonly Action _action;

    public DisposableWrapper([NotNull] Action action)
    {
        ThrowHelper.NotNull(action, nameof(action));

        _action = action;
    }
    
    public void Dispose()
    {
        _action.Invoke();
    }
}