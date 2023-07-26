using System;

namespace Fake.ObjectMapping;

public class NotImplementedAutoMappingProvider : IAutoMappingProvider
{
    public TDestination Map<TSource, TDestination>(object source)
    {
        throw new NotImplementedException($"无法将给定对象({source}) 映射成 {typeof(TDestination).FullName}.");
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        throw new NotImplementedException($"无法将给定对象从 {typeof(TSource).FullName} 映射成 {typeof(TDestination).FullName}.");
    }
}