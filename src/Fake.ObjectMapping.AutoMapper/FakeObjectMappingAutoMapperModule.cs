using System;
using AutoMapper;
using Fake.Modularity;
using Fake.ObjectMapping;
using Fake.ObjectMapping.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[DependsOn(typeof(FakeObjectMappingModule))]
public class FakeObjectMappingAutoMapperModule:FakeModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddServiceRegistrar(new AutoMapperServiceRegistrar());
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(
            ServiceDescriptor.Transient<IObjectMappingProvider, AutoMapperObjectMappingProvider>()
        );
        context.Services.AddSingleton<IMapper>(serviceProvider => CreateMapper(serviceProvider));
    }

    private IMapper CreateMapper(IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }
}