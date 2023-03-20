using Fake.EventBus;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

public class FakeEventBusModule:FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IEventPublisher, FakeEventPublisher>();
    }
}