using System;
using RabbitMQ.Client;

namespace Fake.EventBus.RabbitMQ.Fake.EventBus.RabbitMQ
{
    /// <summary>
    /// RabbitMQ--Channel工厂
    /// </summary>
    public interface IRabbitMqConnector : IDisposable
    {
        /// <summary>
        /// 保持连接活性
        /// </summary>
        void KeepAlive();

        /// <summary>
        /// 创建channel
        /// </summary>
        /// <returns></returns>
        IModel CreateChannel();
    }
}
