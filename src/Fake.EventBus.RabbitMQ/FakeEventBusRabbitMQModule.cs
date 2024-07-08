using Fake.EventBus.Distributed;

// ReSharper disable once CheckNamespace
namespace Fake.EventBus.RabbitMQ;

[DependsOn(typeof(FakeRabbitMqModule))]
public class FakeEventBusRabbitMqModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        context.Services.Configure<RabbitMqEventBusOptions>(configuration);

        context.Services.AddSingleton<IEventBus, RabbitMqEventBus>();
        context.Services.AddSingleton<IDistributedEventBus, RabbitMqEventBus>();
    }
}