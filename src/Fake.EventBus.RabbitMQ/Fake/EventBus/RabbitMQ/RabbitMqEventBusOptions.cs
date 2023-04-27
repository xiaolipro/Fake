namespace Fake.EventBus.RabbitMQ;

public class RabbitMqEventBusOptions
{

    /// <summary>
    /// Broker
    /// </summary>
    public string BrokerName { get; set; }

    /// <summary>
    /// 订阅客户端
    /// </summary>
    public string SubscriptionClientName { get; set; }
        
    #region Qos

    /// <summary>
    /// 消息大小限制
    /// </summary>
    public uint PrefetchSize { get; set; }

    /// <summary>
    /// 每次预取的消息条数
    /// </summary>
    public ushort PrefetchCount { get; set; }

    #endregion

    #region DLX

    /// <summary>
    /// 启动死信
    /// </summary>
    public bool EnableDLX { get; set; }

    /// <summary>
    /// 设置消息过期时间，过期则自动判定为死信
    /// </summary>
    public int MessageTTL { get; set; }

    /// <summary>
    /// 队列最大长度，超出长度则后续消息判定为死信，默认为0无限制
    /// </summary>
    public int QueueMaxLength { get; set; }

    #endregion

}