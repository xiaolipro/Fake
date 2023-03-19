using System;
using System.Collections.Generic;
using System.Text;

namespace Fake.EventBus
{
    /// <summary>
    /// 事件总线
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="event">事件</param>
        void Publish(Event @event);

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <typeparam name="THandler">事件处理者</typeparam>
        void Subscribe<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>;

        /// <summary>
        /// 解阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <typeparam name="THandler">事件处理者</typeparam>
        void Unsubscribe<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>;
    }

    /// <summary>
    /// 动态事件总线
    /// </summary>
    public interface IDynamicEventBus : IEventBus
    {
        /// <summary>
        /// 订阅动态事件
        /// </summary>
        /// <typeparam name="THandler">动态事件处理者</typeparam>
        /// <param name="eventName">动态事件名称</param>
        void SubscribeDynamic<THandler>(string eventName)
            where THandler : IDynamicEventHandler;

        /// <summary>
        /// 解阅动态事件
        /// </summary>
        /// <typeparam name="THandler">动态事件处理者</typeparam>
        /// <param name="eventName">动态事件名称</param>
        void UnsubscribeDynamic<THandler>(string eventName)
            where THandler : IDynamicEventHandler;
    }
}