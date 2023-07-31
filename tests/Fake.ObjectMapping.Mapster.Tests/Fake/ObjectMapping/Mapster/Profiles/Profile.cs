using Mapster;
using Microsoft.Extensions.Options;

namespace Fake.ObjectMapping.Mapster.Profiles;

public class Profile : IProfile
{
    private readonly TypeAdapterConfig _config;

    public Profile(IOptions<FakeMapsterOptions> options) => _config = options.Value.TypeAdapterConfig;

    public TypeAdapterSetter<TSource, TSrc> CreateNewConfig<TSource, TSrc>() => _config.NewConfig<TSource, TSrc>();

    public TypeAdapterSetter<TSource, TSrc> CreateForType<TSource, TSrc>() => _config.ForType<TSource, TSrc>();
}