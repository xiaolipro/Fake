using System.Threading.Channels;
using Fake.EventBus;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

public class FakeEventBusModule:FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IEventPublisher, EventPublisher>();
        
        context.Services.AddSingleton<IEventBus, LocalEventBus>();
        context.Services.Configure<LocalEventBusOptions>(options =>
        {
            options.Capacity = 100;
            options.FullMode = BoundedChannelFullMode.Wait;  // channel满了阻塞
        });
    }
}