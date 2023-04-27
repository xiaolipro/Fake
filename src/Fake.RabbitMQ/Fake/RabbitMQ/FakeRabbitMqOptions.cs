using System.Collections.Generic;
using JetBrains.Annotations;
using RabbitMQ.Client;

namespace Fake.RabbitMQ;

public class FakeRabbitMqOptions
{
    public string DefaultConnectionName { get; set; } = "Default";

    public Dictionary<string, ConnectionFactory> Connections { get; set; }

    [NotNull]
    public ConnectionFactory Default
    {
        get => Connections[DefaultConnectionName];
        private set => Connections[DefaultConnectionName] = value;
    }

    public FakeRabbitMqOptions()
    {
        Default = new ConnectionFactory();
    }

    public ConnectionFactory GetOrDefault(string connectionName)
    {
        return Connections.TryGetValue(connectionName, out var connectionFactory) ? connectionFactory : Default;
    }
}