using Mapster;

namespace Fake.ObjectMapping.Mapster.Profiles;

public class Profile : IProfile
{
    private readonly TypeAdapterConfig _config;

    public Profile() => _config = TypeAdapterConfig.GlobalSettings;

    public TypeAdapterSetter<TSource, TSrc> CreateConfig<TSource, TSrc>() => _config.NewConfig<TSource, TSrc>();
}