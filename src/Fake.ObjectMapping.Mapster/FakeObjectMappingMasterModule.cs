using Fake.Modularity;
using Fake.ObjectMapping;
using Fake.ObjectMapping.Mapster;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[DependsOn(typeof(FakeObjectMappingModule))]
public class FakeObjectMappingMasterModule:FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(
            ServiceDescriptor.Transient<IObjectMappingProvider, MapsterObjectMappingProvider>()
        );
    }
}