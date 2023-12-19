namespace Fake.EventBus.RabbitMQ;

public class RabbitMqEventBusOptions
{
    public string? ConnectionName { get; set; } = "Default";

    /// <summary>
    /// Broker
    /// </summary>
    public string BrokerName { get; set; }

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
    public bool EnableDlx { get; set; }

    /// <summary>
    /// 设置消息过期时间，过期则自动判定为死信
    /// </summary>
    public int MessageTtl { get; set; }

    /// <summary>
    /// 队列最大长度，超出长度则后续消息判定为死信，默认为0无限制
    /// </summary>
    public int QueueMaxLength { get; set; }

    #endregion

    public RabbitMqEventBusOptions()
    {
        BrokerName = "Fake.Exchange.EventBus"; // 交换机名称
        PrefetchSize = 0; // Prefetch消息大小无限制
        PrefetchCount = 1; // 每次预取1条
        EnableDlx = true; // 启用DLX
        MessageTtl = 0; // 无限制
    }
}