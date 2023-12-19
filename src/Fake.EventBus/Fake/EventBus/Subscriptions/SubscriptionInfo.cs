using System;

namespace Fake.EventBus.Subscriptions
{
    public class SubscriptionInfo
    {
        /// <summary>
        /// 是否是动态事件
        /// </summary>
        public bool IsDynamic => EventType == null;

        /// <summary>
        /// 事件名
        /// </summary>
        public string EventName { get; }

        /// <summary>
        /// 事件类型
        /// </summary>
        public Type? EventType { get; }

        /// <summary>
        /// 事件处理者类型
        /// </summary>
        public Type HandlerType { get; }

        public SubscriptionInfo(string eventName, Type handlerType)
        {
            EventName = eventName;
            HandlerType = handlerType;
        }

        public SubscriptionInfo(Type eventType, Type handlerType)
        {
            EventName = nameof(eventType);
            EventType = eventType;
            HandlerType = handlerType;
        }

        public static SubscriptionInfo Dynamic(string eventName, Type handlerType) => new(eventName, handlerType);

        public static SubscriptionInfo Typed(Type eventType, Type handlerType) => new(eventType, handlerType);
    }
}