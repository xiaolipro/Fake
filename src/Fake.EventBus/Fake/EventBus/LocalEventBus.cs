using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Fake.EventBus.Events;
using Fake.EventBus.Subscriptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fake.EventBus;

public class LocalEventBus : IEventBus
{
    private readonly ILogger<LocalEventBus> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ISubscriptionsManager _subscriptionsManager;

    private readonly Channel<IEvent> _channel; // 事件存储源

    public LocalEventBus(ILogger<LocalEventBus> logger,
        IServiceScopeFactory serviceScopeFactory,
        ISubscriptionsManager subscriptionsManager,
        IOptions<LocalEventBusOptions> options)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _subscriptionsManager = subscriptionsManager;

        var eventBusOptions = options.Value;
        var channelOptions = new BoundedChannelOptions(eventBusOptions.Capacity)
        {
            FullMode = eventBusOptions.FullMode
        };
        _channel = Channel.CreateBounded<IEvent>(channelOptions);

        StartBasicConsume(eventBusOptions.ConsumerThreads);
    }


    public Task PublishAsync(IEvent @event)
    {
        return _channel.Writer.WriteAsync(@event).AsTask();
    }

    public void Subscribe<TEvent, THandler>() where TEvent : IEvent where THandler : IEventHandler<TEvent>
    {
        var eventName = _subscriptionsManager.GetEventName<TEvent>();
        _logger.LogInformation("{EventHandler}订阅了事件{EventName}", typeof(THandler).GetName(), eventName);

        _subscriptionsManager.AddSubscription<TEvent, THandler>();
    }

    private void StartBasicConsume(int consumerThreads = 1)
    {
        var taskFactory = new TaskFactory(TaskScheduler.Current);
        for (int i = 0; i < consumerThreads; i++)
        {
            taskFactory.StartNew(async () =>
            {
                while (true)
                {
                    // Consumer patterns
                    // May throw ChannelClosedException if the parent channel's writer signals complete.
                    // Note. This code will throw an exception if the channel is closed.
                    // Details see: https://learn.microsoft.com/en-us/dotnet/core/extensions/channels
                    var @event = await _channel.Reader.ReadAsync(CancellationToken.None);

                    await ProcessingAsync(@event, CancellationToken.None);
                }
                // ReSharper disable once FunctionNeverReturns
            }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
    }

    public void Unsubscribe<TEvent, THandler>() where TEvent : IEvent where THandler : IEventHandler<TEvent>
    {
        var eventName = _subscriptionsManager.GetEventName<TEvent>();

        _logger.LogInformation("{EventHandler}解除了对事件{EventName}的订阅", typeof(THandler).GetName(), eventName);

        _subscriptionsManager.RemoveSubscription<TEvent, THandler>();
    }

    private async Task ProcessingAsync(IEvent @event, CancellationToken cancellationToken)
    {
        string eventName = @event.GetType().GetName();
        // 空订阅
        if (!_subscriptionsManager.HasSubscriptions(eventName))
        {
            _logger.LogWarning("{EventName}没有任何订阅者", eventName);
            return;
        }

        var subscriptionInfos = _subscriptionsManager.GetSubscriptionInfos(eventName);

        // 广播
        using var scope = _serviceScopeFactory.CreateScope();
        foreach (var subscriptionInfo in subscriptionInfos)
        {
            var eventType = _subscriptionsManager.GetEventTypeByName(eventName);
            var handler = scope.ServiceProvider.GetService(subscriptionInfo.HandlerType);
            if (handler == null)
            {
                _logger.LogWarning("{EventHandlerName}没有实现`IIntegrationEventHandler`", nameof(handler));
                continue;
            }

            var handle = typeof(IEventHandler<>)
                .MakeGenericType(eventType)
                .GetMethod(nameof(IEventHandler<IEvent>.HandleAsync));

            // see：https://stackoverflow.com/questions/22645024/when-would-i-use-task-yield
            await Task.Yield();
            _logger.LogDebug("正在处理集成事件: {EventName}", eventName);
            handle?.Invoke(handler, new object[] { @event, cancellationToken });
        }
    }
}