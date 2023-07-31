using Fake.Modularity;
using Fake.ObjectMapping.Mapster;
using Fake.ObjectMapping.Mapster.Profiles;
using Fake.ObjectMapping.Models;
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
        });

        context.Services.AddSingleton<MyProfile>();
    }
}