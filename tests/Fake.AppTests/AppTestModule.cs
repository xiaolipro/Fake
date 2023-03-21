using Application.DomainEventHandlers.OrderStartedDomainEventHandlers;
using Domain.Events;
using Fake.Autofac;
using Fake.EventBus.Events;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

[DependsOn(typeof(FakeAutofacModule))]
[DependsOn(typeof(FakeDddDomainModule))]
public class FakeAppTestModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton(typeof(IEventHandler<OrderStartedDomainEvent>),
            typeof(ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler));
    }
}