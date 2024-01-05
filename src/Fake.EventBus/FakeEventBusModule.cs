using Fake.EventBus.Subscriptions;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.EventBus;

public class FakeEventBusModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IEventPublisher, EventPublisher>();

        context.Services.AddSingleton<IEventBus, LocalEventBus>();
        context.Services.AddSingleton<ISubscriptionsManager, InMemorySubscriptionsManager>();
        context.Services.Configure<LocalEventBusOptions>(_ => { });
    }
}