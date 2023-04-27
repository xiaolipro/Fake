using System.Threading.Channels;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.EventBus;
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