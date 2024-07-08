using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fake.EventBus.Local;

public abstract class EventHandlerWrapper
{
    public abstract Task HandleAsync(EventBase @event, IServiceProvider serviceProvider,
        Func<IEnumerable<EventHandlerExecutor>, EventBase, CancellationToken, Task> publish,
        CancellationToken cancellationToken);
}