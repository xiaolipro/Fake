using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fake.EventBus.Local;

public record EventHandlerExecutor(object HandlerInstance, Func<EventBase, CancellationToken, Task> HandlerCallback)
{
    public object HandlerInstance { get; } = HandlerInstance;
    public Func<EventBase, CancellationToken, Task> HandlerCallback { get; } = HandlerCallback;
}