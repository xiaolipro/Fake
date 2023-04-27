using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Fake.EventBus.RabbitMQ;

/// <summary>
/// RabbitMQ连接器、Channel工厂
/// </summary>
/// <remarks>复用同一个IConnection，减少连接的高昂开销</remarks>
public class RabbitMqConnector : IRabbitMqConnector
{
    private readonly ILogger<RabbitMqConnector> _logger;
    private readonly IConnectionFactory _connectionFactory;
    private readonly object _lock = new object();

    private IConnection _connection;
    private bool _disposed;


    public RabbitMqConnector(ILogger<RabbitMqConnector> logger, IOptions<RabbitMQClientOptions> config)
    {
        _logger = logger;
        var clientConfig = config.Value;
        _connectionFactory = GetConnectionFactory(clientConfig);
    }


    public IModel CreateChannel()
    {
        if (!IsConnected)
        {
            ConnectRabbitMq();
        }

        return _connection.CreateModel();
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;

        try
        {
            // 事件解绑
            _connection.ConnectionBlocked -= OnConnectionBlocked;
            _connection.CallbackException -= OnCallbackException;
            _connection.ConnectionShutdown -= OnConnectionShutdown;
            _connection.Dispose();
        }
        catch (IOException ex)
        {
            _logger.LogCritical(ex.ToString());
        }
    }

    /// <summary>
    /// 保持连接活性
    /// </summary>
    public void KeepAlive()
    {
        if (!IsConnected)
        {
            ConnectRabbitMq();
        }
    }

    /// <summary>
    /// 是连接状态
    /// </summary>
    private bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

    /// <summary>
    /// 重连
    /// </summary>
    /// <exception cref="Exception">RabbitMQ connections could not be created and opened</exception>
    private void ConnectRabbitMq()
    {
        _logger.LogInformation("正在尝试连接RabbitMQ客户端");

        lock (_lock)
        {
            _connection = _connectionFactory.CreateConnection();

            if (!IsConnected) throw new Exception("致命错误：无法创建和打开RabbitMQ连接");

            _connection.ConnectionShutdown += OnConnectionShutdown;
            _connection.CallbackException += OnCallbackException;

            // RabbitMQ出于自身保护策略，通过阻塞方式限制写入，导致了生产者应用“假死”，不再对外服务。
            // 比若说CPU/IO/RAM资源下降，队列堆积，导致堵塞，就会触发这个事件
            _connection.ConnectionBlocked += OnConnectionBlocked;

            _logger.LogInformation("{HostName}获得到了RabbitMQ客户端的持久连接", _connection.Endpoint.HostName);
        }

    }

    /// <summary>
    /// 获取连接工厂
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    private IConnectionFactory GetConnectionFactory(RabbitMQClientOptions config)
    {
        var connectionFactory = new ConnectionFactory()
        {
            DispatchConsumersAsync = true
        };

        if (!string.IsNullOrWhiteSpace(config.HostName))
        {
            connectionFactory.HostName = config.HostName;
        }

        if (!string.IsNullOrWhiteSpace(config.HostName))
        {
            connectionFactory.Port = config.Port;

        }

        if (!string.IsNullOrWhiteSpace(config.HostName))
        {
            connectionFactory.UserName = config.UserName;
        }

        if (!string.IsNullOrWhiteSpace(config.HostName))
        {
            connectionFactory.Password = config.Password;
        }

        if (!string.IsNullOrWhiteSpace(config.HostName))
        {
            connectionFactory.VirtualHost = config.VirtualHost;
        }

        return connectionFactory;
    }

    /// <summary>
    /// 连接阻塞时
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
    {
        if (_disposed) return;

        _logger.LogWarning("RabbitMQ连接被阻止，正在尝试重新连接。。。");

        ConnectRabbitMq();
    }

    /// <summary>
    /// 其他时间执行发生异常时
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OnCallbackException(object sender, CallbackExceptionEventArgs e)
    {
        if (_disposed) return;

        _logger.LogWarning("RabbitMQ连接发生异常：{Message}，正在尝试重新连接。。。", e.Exception.Message);

        ConnectRabbitMq();
    }

    /// <summary>
    /// 断开连接时
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="reason"></param>
    void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
    {
        if (_disposed) return;

        _logger.LogWarning("RabbitMQ连接已被关闭，正在尝试重新连接。。。");

        ConnectRabbitMq();
    }

}