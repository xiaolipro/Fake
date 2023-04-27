using Fake.EventBus.RabbitMQ.Fake.EventBus.RabbitMQ;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.EventBus.RabbitMQ;

public class FakeEventBusRabbitMqModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfigurationOrDefault();
        context.Services.Configure<RabbitMQClientOptions>(configuration.GetSection("ConnectionStrings:RabbitMQ"));

        context.Services.Configure<RabbitMqEventBusOptions>(options =>
        {
            options.BrokerName = "Fake.Exchange.EventBus"; // 交换机名称
            options.PrefetchSize = 0; // Prefetch消息大小无限制
            options.PrefetchCount = 1; // 每次预取1条
            options.EnableDLX = true; // 启用DLX
            options.MessageTTL = 0; // 无限制
        });
        context.Services.AddSingleton<IEventBus, RabbitMqEventBus>();
    }
}