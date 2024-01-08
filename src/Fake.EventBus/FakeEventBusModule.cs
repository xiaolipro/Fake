using Fake.EventBus.Subscriptions;
using Fake.Modularity;
using Fake.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.EventBus;

[DependsOn(typeof(FakeUnitOfWorkModule))]
public class FakeEventBusModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IEventPublisher, EventPublisher>();

        context.Services.AddSingleton<LocalEventBus>();
        context.Services.Configure<LocalEventBusOptions>(_ => { });

        context.Services.AddSingleton<IEventBus, LocalEventBus>();
        context.Services.AddSingleton<ISubscriptionsManager, InMemorySubscriptionsManager>();
    }
}