using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.EventBus.Local;

public class EventHandlerWrapperImpl<TEvent> : EventHandlerWrapper where TEvent : EventBase
{
    public override Task HandleAsync(EventBase @event, IServiceProvider serviceProvider,
        Func<IEnumerable<EventHandlerExecutor>, EventBase, CancellationToken, Task> publish,
        CancellationToken cancellationToken)
    {
        var handlers = serviceProvider
            .GetServices<IEventHandler<TEvent>>()
            .Select(handler => new EventHandlerExecutor(handler,
                (theEvent, theToken) => handler.HandleAsync((TEvent)theEvent, theToken)));

        return publish(handlers, @event, cancellationToken);
    }
}