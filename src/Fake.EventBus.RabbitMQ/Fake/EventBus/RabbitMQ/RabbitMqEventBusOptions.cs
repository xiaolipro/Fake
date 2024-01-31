﻿namespace Fake.EventBus.RabbitMQ;

public class RabbitMqEventBusOptions
{
    /// <summary>
    /// RabbitMQ连接名，若为空则使用默认连接<see cref="FakeRabbitMqOptions"/>
    /// </summary>
    public string? ConnectionName { get; set; }

    /// <summary>
    /// Broker 交换机名称
    /// </summary>
    public string BrokerName { get; set; } = "Fake.Exchange.EventBus";

    #region Qos

    /// <summary>
    /// 消息大小限制，默认无限制
    /// </summary>
    public uint PrefetchSize { get; set; }

    /// <summary>
    /// 每次预取的消息条数
    /// </summary>
    public ushort PrefetchCount { get; set; } = 1;

    #endregion

    #region DLX

    /// <summary>
    /// 启动死信
    /// </summary>
    public bool EnableDlx { get; set; } = true;

    /// <summary>
    /// 设置消息过期时间，过期则自动判定为死信，默认无限制
    /// </summary>
    public int MessageTtl { get; set; }

    /// <summary>
    /// 队列最大长度，超出长度则后续消息判定为死信，默认无限制
    /// </summary>
    public int QueueMaxLength { get; set; }

    #endregion
}