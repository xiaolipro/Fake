using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fake.EventBus.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.EventBus.Wrappers;

public abstract class EventHandlerWrapper
{
    public abstract Task Handle(IEvent @event, IServiceProvider serviceProvider,
        Func<IEnumerable<EventHandlerExecutor>, IEvent, CancellationToken, Task> publish,
        CancellationToken cancellationToken);
}

public class EventHandlerWrapperImpl<TEvent> : EventHandlerWrapper where TEvent : IEvent
{
    public override Task Handle(IEvent @event, IServiceProvider serviceProvider,
        Func<IEnumerable<EventHandlerExecutor>, IEvent, CancellationToken, Task> publish,
        CancellationToken cancellationToken)
    {
        var handlers = serviceProvider
            .GetServices<IEventHandler<TEvent>>()
            .Select(handler =>
                new EventHandlerExecutor(handler, (theEvent, theToken) => handler.Handle((TEvent)theEvent, theToken)));

        return publish(handlers, @event, cancellationToken);
    }
}