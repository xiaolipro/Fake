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
/// 简易的发布者交互模式
/// </summary>
public class FakeEventPublisher : IEventPublisher
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ConcurrentDictionary<Type, EventHandlerWrapper> _eventHandlers;

    public FakeEventPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _eventHandlers = new ConcurrentDictionary<Type, EventHandlerWrapper>();
    }

    public Task Publish([NotNull]IEvent @event, CancellationToken cancellationToken = default)
    {
        ThrowHelper.ThrowIfNull(@event, nameof(@event));

        return PublishEvent(@event, cancellationToken);
    }

    private Task PublishEvent(IEvent @event, CancellationToken cancellationToken)
    {
        var eventHandler = _eventHandlers.GetOrAdd(@event.GetType(), eventType =>
        {
            var wrapper =
                ReflectionHelper.CreateInstance<EventHandlerWrapper>(
                    typeof(EventHandlerWrapperImpl<>).MakeGenericType(eventType));

            if (wrapper == null)
            {
                throw new FakeException($"无法创建类型{eventType}的{nameof(EventHandlerWrapper)}");
            }

            return wrapper;
        });


        return eventHandler.Handle(@event, _serviceProvider, PublishCore, cancellationToken);
    }

    protected virtual async Task PublishCore(IEnumerable<EventHandlerExecutor> eventHandlerExecutors, IEvent @event, CancellationToken cancellationToken)
    {
        foreach (var eventHandlerExecutor in eventHandlerExecutors)
        {
            await eventHandlerExecutor.HandlerCallback(@event, cancellationToken);
        }
    }

}