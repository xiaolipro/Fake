using System;
using AutoMapper;
using AutoMapper.Internal;
using Fake.Helpers;
using Fake.Modularity;
using Fake.ObjectMapping;
using Fake.ObjectMapping.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

[DependsOn(typeof(FakeObjectMappingModule))]
public class FakeObjectMappingAutoMapperModule : FakeModule
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
        context.Services.AddSingleton<IMapper, Mapper>(CreateMapper);
    }

    private Mapper CreateMapper(IServiceProvider serviceProvider)
    {
        var options = serviceProvider.GetRequiredService<IOptions<FakeAutoMapperOptions>>().Value;

        var mapperConfiguration = new MapperConfiguration(expression =>
        {
            foreach (var configurator in options.Configurators)
            {
                configurator(expression, serviceProvider);
            }
        });

        // validate
        foreach (var profileType in options.ValidatingProfile)
        {
            mapperConfiguration.Internal().AssertConfigurationIsValid(profileType.FullName);
        }

        // serviceCtor: Factory method to create formatters, resolvers and type converters 用我们的根容器创建
        return new Mapper(mapperConfiguration, serviceCtor: serviceProvider.GetRequiredService);
    }
}