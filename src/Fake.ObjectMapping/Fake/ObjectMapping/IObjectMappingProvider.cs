namespace Fake.ObjectMapping;

public interface IObjectMappingProvider
{
    TDestination Map<TSource, TDestination>(object source);

    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
}