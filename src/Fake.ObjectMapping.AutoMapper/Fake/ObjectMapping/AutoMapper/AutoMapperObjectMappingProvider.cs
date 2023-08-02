using AutoMapper;

namespace Fake.ObjectMapping.AutoMapper;

public class AutoMapperObjectMappingProvider:IObjectMappingProvider
{
    private readonly IMapper _mapper;

    public AutoMapperObjectMappingProvider(IMapper mapper)
    {
        _mapper = mapper;
    }
    public TDestination Map<TSource, TDestination>(object source)
    {
        return _mapper.Map<TDestination>(source);
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        return _mapper.Map(source, destination);
    }
}