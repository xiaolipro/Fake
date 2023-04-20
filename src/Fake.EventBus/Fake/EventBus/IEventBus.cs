﻿using Fake.EventBus.Events;

namespace Fake.EventBus;

/// <summary>
/// 事件总线--发布订阅模式
/// </summary>
public interface IEventBus : IEventPublisher
{
    /// <summary>
    /// 订阅事件
    /// </summary>
    /// <typeparam name="TEvent">事件</typeparam>
    /// <typeparam name="THandler">事件处理者</typeparam>
    void Subscribe<TEvent, THandler>()
        where TEvent : IEvent
        where THandler : IEventHandler<TEvent>;

    /// <summary>
    /// 解阅事件
    /// </summary>
    /// <typeparam name="TEvent">事件</typeparam>
    /// <typeparam name="THandler">事件处理者</typeparam>
    void Unsubscribe<TEvent, THandler>()
        where TEvent : IEvent
        where THandler : IEventHandler<TEvent>;
}