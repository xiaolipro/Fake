namespace Fake.EventBus.RabbitMQ.Fake.EventBus.RabbitMQ
{
    public class RabbitMqEventBusOptions
    {

        /// <summary>
        /// Broker
        /// </summary>
        public string BrokerName { get; set; } = "Fake.Exchange.EventBus";

        /// <summary>
        /// 订阅客户端
        /// </summary>
        public string SubscriptionClientName { get; set; }

        /// <summary>
        /// 发布失败重试次数，默认5次
        /// </summary>
        public int PublishFailureRetryCount { get; set; } = 5;


        #region Qos

        /// <summary>
        /// 消息大小限制，默认0无限制
        /// </summary>
        public uint PrefetchSize { get; set; } = 0;

        /// <summary>
        /// 每次预取的消息条数，默认1条
        /// </summary>
        public ushort PrefetchCount { get; set; } = 1;

        #endregion

        #region DLX

        /// <summary>
        /// 启动DLX，默认true
        /// </summary>
        public bool EnableDLX { get; set; } = true;

        /// <summary>
        /// 设置消息过期时间，过期则自动判定为死信，默认为0无限制
        /// </summary>
        public int MessageTTL { get; set; }

        /// <summary>
        /// 队列最大长度，超出长度则后续消息判定为死信，默认为0无限制
        /// </summary>
        public int QueueMaxLength { get; set; }

        #endregion

    }
}
