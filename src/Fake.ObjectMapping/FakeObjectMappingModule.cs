using System;
using System.Collections.Generic;
using Fake.Modularity;
using Fake.ObjectMapping;
using Microsoft.Extensions.DependencyInjection;

public class FakeObjectMappingModule:FakeModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnServiceExposing(exposingContext =>
        {
            foreach (var interfaceType in exposingContext.ImplementationType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IObjectMapper<,>))
                {
                    exposingContext.ExposedServiceTypes.TryAdd(interfaceType);
                }
            }
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IObjectMappingProvider, NotImplementedObjectMappingProvider>();
        context.Services.AddTransient<IObjectMapper, DefaultObjectMapper>();
    }
}