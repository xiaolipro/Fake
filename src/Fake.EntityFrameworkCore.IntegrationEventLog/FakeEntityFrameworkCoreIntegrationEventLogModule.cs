using Fake.Modularity;

// ReSharper disable once CheckNamespace
namespace Fake.EntityFrameworkCore.IntegrationEventLog;

[DependsOn(typeof(FakeEntityFrameworkCoreModule))]
public class FakeEntityFrameworkCoreIntegrationEventLogModule:FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        
    }
}