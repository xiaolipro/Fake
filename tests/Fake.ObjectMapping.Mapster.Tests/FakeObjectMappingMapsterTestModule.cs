using Fake.Modularity;
using Fake.ObjectMapping.Mapster;
using Mapster;
using Microsoft.Extensions.DependencyInjection;


[DependsOn(typeof(FakeObjectMappingMasterModule))]
public class FakeObjectMappingMapsterTestModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<FakeMapsterOptions>(options =>
        {
            options.TypeAdapterConfig.Default.PreserveReference(true);
            options.AddMaps<FakeObjectMappingMapsterTestModule>();
        });
    }
}