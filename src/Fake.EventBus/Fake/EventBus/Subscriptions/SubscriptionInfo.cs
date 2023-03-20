using System;

namespace Fake.EventBus.Subscriptions
{
    public class SubscriptionInfo
    {
        /// <summary>
        /// 是否是动态事件
        /// </summary>
        public bool IsDynamic { get; }

        /// <summary>
        /// 事件名
        /// </summary>
        public string EventName { get; }

        /// <summary>
        /// 事件处理者类型
        /// </summary>
        public Type HandlerType { get; }

        public SubscriptionInfo(bool isDynamic, string eventName, Type handlerType)
        {
            IsDynamic = isDynamic;
            EventName = eventName;
            HandlerType = handlerType;
        }

        public static SubscriptionInfo Dynamic(string eventName, Type handlerType) =>
            new SubscriptionInfo(true, eventName, handlerType);

        public static SubscriptionInfo Typed(string eventName, Type handlerType) =>
            new SubscriptionInfo(false, eventName, handlerType);
    }
}
