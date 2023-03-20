using System;
using System.Threading;
using System.Threading.Tasks;
using Fake.EventBus.Events;

namespace Fake.EventBus.Wrappers;

public record EventHandlerExecutor(object HandlerInstance, Func<IEvent, CancellationToken, Task> HandlerCallback)
{
    public object HandlerInstance { get; } = HandlerInstance;
    public Func<IEvent, CancellationToken, Task> HandlerCallback { get; } = HandlerCallback;
}