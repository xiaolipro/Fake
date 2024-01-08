using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fake.EventBus.Events;
using Fake.EventBus.Wrappers;
using Fake.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fake.EventBus;

public class LocalEventBus(
    ILogger<LocalEventBus> logger,
    IServiceScopeFactory serviceScopeFactory)
    : IEventBus
{
    private readonly ConcurrentDictionary<Type, EventHandlerWrapper> _eventHandlers = new();

    public virtual async Task PublishAsync(EventBase @event)
    {
        ThrowHelper.ThrowIfNull(@event, nameof(@event));

        var eventHandler = _eventHandlers.GetOrAdd(@event.GetType(), eventType =>
        {
            var wrapper = ReflectionHelper.CreateInstance<EventHandlerWrapper>(
                typeof(EventHandlerWrapperImpl<>).MakeGenericType(eventType));

            if (wrapper == null)
            {
                throw new FakeException($"无法为事件{eventType}创建{nameof(EventHandlerWrapper)}");
            }

            return wrapper;
        });

        using var scope = serviceScopeFactory.CreateScope();
        await eventHandler.HandleAsync(@event, scope.ServiceProvider, ProcessingEventAsync, default);
    }

    protected virtual async Task ProcessingEventAsync(IEnumerable<EventHandlerExecutor> eventHandlerExecutors,
        EventBase @event,
        CancellationToken cancellationToken)
    {
        // 广播事件
        foreach (var eventHandlerExecutor in eventHandlerExecutors)
        {
            logger.LogDebug("正在处理: {Event}", @event.ToString());
            await eventHandlerExecutor.HandlerCallback(@event, cancellationToken);
        }
    }

    public virtual void Subscribe<TEvent, THandler>() where TEvent : EventBase where THandler : IEventHandler<TEvent>
    {
        // 本地事件总线不需要订阅
    }

    public virtual void Unsubscribe<TEvent, THandler>() where TEvent : EventBase where THandler : IEventHandler<TEvent>
    {
        // 本地事件总线不需要订阅
    }
}