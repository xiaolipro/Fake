using System.Runtime.CompilerServices;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Options;

namespace Fake.ObjectMapping.Mapster.Profiles;

public class Profile : IProfile
{
    private readonly TypeAdapterConfig _config;

    public Profile(IOptions<FakeMapsterOptions> options) => _config = options.Value.TypeAdapterConfig;

    public TypeAdapterSetter<TSource, TSrc> CreateConfig<TSource, TSrc>() => _config.NewConfig<TSource, TSrc>();
}