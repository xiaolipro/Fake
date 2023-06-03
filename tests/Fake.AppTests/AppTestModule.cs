using Application.DomainEventHandlers.BuyerAndPaymentMethodVerifiedEvent;
using Application.DomainEventHandlers.OrderStartedEvent;
using Domain.Events;
using Fake.Autofac;
using Fake.Ddd.Domain;
using Fake.EventBus.Events;
using Fake.Helpers;
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

        context.Services.AddSingleton(typeof(IEventHandler<BuyerAndPaymentMethodVerifiedDomainEvent>),
            typeof(UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler));

        context.Services.AddTransient<AppTestDataBuilder>();
    }

    public override void ConfigureApplication(ApplicationConfigureContext context)
    {
        AsyncHelper.RunSync(() => context.ServiceProvider.GetRequiredService<AppTestDataBuilder>().BuildAsync());
    }
}