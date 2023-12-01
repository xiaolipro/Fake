using System;
using RabbitMQ.Client;

namespace Fake.RabbitMQ;

/// <summary>
/// 封装 RabbitMQ 连接池，用于获取 RabbitMQ 连接。
/// </summary>
public interface IRabbitMqConnectionPool : IDisposable
{
    IConnection Get(string? connectionName = null);
}