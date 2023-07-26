using System;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.ObjectMapping;

public class DefaultObjectMapper : IObjectMapper
{
    public IAutoMappingProvider AutoMappingProvider { get; }
    protected IServiceProvider ServiceProvider { get; }

    public DefaultObjectMapper(IAutoMappingProvider autoMappingProvider, IServiceProvider serviceProvider)
    {
        AutoMappingProvider = autoMappingProvider;
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
        
        // 自动映射
        return AutoMappingProvider.Map<TSource, TDestination>(source);
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
        
        // 自动映射
        return AutoMappingProvider.Map(source, destination);
    }
}