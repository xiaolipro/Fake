using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fake.EventBus.Events;

namespace Fake.EventBus.Wrappers;

public abstract class EventHandlerWrapper
{
    public abstract Task HandleAsync(EventBase @event, IServiceProvider serviceProvider,
        Func<IEnumerable<EventHandlerExecutor>, EventBase, CancellationToken, Task> publish,
        CancellationToken cancellationToken);
}