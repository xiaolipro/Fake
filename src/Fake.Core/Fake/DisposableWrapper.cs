namespace Fake;

public class DisposableWrapper:IDisposable
{
    private readonly Action _action;

    public DisposableWrapper([NotNull] Action action)
    {
        ThrowHelper.ThrowIfNull(action, nameof(action));

        _action = action;
    }
    
    public void Dispose()
    {
        _action.Invoke();
    }
}

public class AsyncDisposableWrapper:IAsyncDisposable
{
    private readonly Task _task;

    public AsyncDisposableWrapper([NotNull] Task task)
    {
        ThrowHelper.ThrowIfNull(task, nameof(task));

        _task = task;
    }

    public async ValueTask DisposeAsync()
    {
        await _task;
    }
}