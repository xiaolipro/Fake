using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fake.EventBus.Events;
using Fake.EventBus.Wrappers;
using Fake.Reflection;
using JetBrains.Annotations;

namespace Fake.EventBus;

/// <summary>
/// 简易的发布者模式
/// </summary>
public class EventPublisher : IEventPublisher
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ConcurrentDictionary<Type, EventHandlerWrapper> _eventHandlers;

    public EventPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _eventHandlers = new ConcurrentDictionary<Type, EventHandlerWrapper>();
    }

    public void Publish([NotNull] IEvent @event)
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


        eventHandler.Handle(@event, _serviceProvider, PublishCore, default);
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