namespace Fake.ObjectMapping;

public interface IAutoMappingProvider
{
    TDestination Map<TSource, TDestination>(object source);

    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
}