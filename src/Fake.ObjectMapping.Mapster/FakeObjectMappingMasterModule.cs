using System;
using Fake.Modularity;
using Fake.ObjectMapping;
using Fake.ObjectMapping.Mapster;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

[DependsOn(typeof(FakeObjectMappingModule))]
public class FakeObjectMappingMasterModule:FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(
            ServiceDescriptor.Transient<IObjectMappingProvider, MapsterObjectMappingProvider>()
        );
        
        context.Services.AddSingleton<IMapper, Mapper>(CreateMapper);
    }

    private Mapper CreateMapper(IServiceProvider serviceProvider)
    {
        var options = serviceProvider.GetRequiredService<IOptions<FakeMapsterOptions>>().Value;

        return new Mapper(options.TypeAdapterConfig);
    }
}