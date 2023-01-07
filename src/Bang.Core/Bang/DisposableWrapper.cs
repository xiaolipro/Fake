﻿namespace Bang;

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