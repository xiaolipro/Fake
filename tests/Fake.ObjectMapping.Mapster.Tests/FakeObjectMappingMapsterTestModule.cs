using Fake.Modularity;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.ObjectMapping.Mapster.Tests;

[DependsOn(typeof(FakeObjectMappingMasterModule))]
public class FakeObjectMappingMapsterTestModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<FakeMapsterOptions>(options =>
        {
            // options.TypeAdapterConfig.RequireExplicitMapping = true;
            options.TypeAdapterConfig.Default.PreserveReference(true);
            options.Scan<FakeObjectMappingMapsterTestModule>();
        });
    }
}