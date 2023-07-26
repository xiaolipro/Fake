namespace Fake.ObjectMapping.AutoMapper;

public class AutoMapperObjectMappingProvider:IObjectMappingProvider
{
    public TDestination Map<TSource, TDestination>(object source)
    {
        throw new System.NotImplementedException();
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        throw new System.NotImplementedException();
    }
}