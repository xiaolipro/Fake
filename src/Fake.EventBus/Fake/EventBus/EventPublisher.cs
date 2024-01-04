using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fake.EventBus.Events;
using Fake.EventBus.Wrappers;
using Fake.Helpers;

namespace Fake.EventBus;

public class EventPublisher(IServiceProvider serviceProvider) : IEventPublisher
{
    private readonly ConcurrentDictionary<Type, EventHandlerWrapper> _eventHandlers = new();

    public async Task PublishAsync(IEvent @event)
    {
        ThrowHelper.ThrowIfNull(@event, nameof(@event));

        var eventHandler = _eventHandlers.GetOrAdd(@event.GetType(), eventType =>
        {
            var wrapper = ReflectionHelper.CreateInstance<EventHandlerWrapper>(
                typeof(EventHandlerWrapperImpl<>).MakeGenericType(eventType));

            if (wrapper == null)
            {
                throw new FakeException($"无法为类型{eventType}创建{nameof(EventHandlerWrapper)}");
            }

            return wrapper;
        });


        await eventHandler.Handle(@event, serviceProvider, PublishCore, default);
    }

    protected virtual async Task PublishCore(IEnumerable<EventHandlerExecutor> eventHandlerExecutors, IEvent @event,
        CancellationToken cancellationToken)
    {
        foreach (var eventHandlerExecutor in eventHandlerExecutors)
        {
            await eventHandlerExecutor.HandlerCallback(@event, cancellationToken);
        }
    }
}