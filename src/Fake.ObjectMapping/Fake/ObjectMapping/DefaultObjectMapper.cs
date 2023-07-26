using System;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.ObjectMapping;

public class DefaultObjectMapper : IObjectMapper
{
    public IObjectMappingProvider ObjectMappingProvider { get; }
    protected IServiceProvider ServiceProvider { get; }

    public DefaultObjectMapper(IObjectMappingProvider objectMappingProvider, IServiceProvider serviceProvider)
    {
        ObjectMappingProvider = objectMappingProvider;
        ServiceProvider = serviceProvider;
    }
    public virtual TDestination Map<TSource, TDestination>(object source)
    {
        if (source == null) return default;

        // 优先使用特定映射器
        var specificMapper = ServiceProvider.GetService<ISpecificObjectMapper<TSource, TDestination>>();
        if (specificMapper != null)
        {
            return specificMapper.Map(source);
        }
        
        return ObjectMappingProvider.Map<TSource, TDestination>(source);
    }

    public virtual TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        if (source == null) return default;
        
        // 优先使用特定映射器
        var specificMapper = ServiceProvider.GetService<ISpecificObjectMapper<TSource, TDestination>>();
        if (specificMapper != null)
        {
            return specificMapper.Map(source);
        }
        
        return ObjectMappingProvider.Map(source, destination);
    }
}