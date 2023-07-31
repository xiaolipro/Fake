using Fake.ObjectMapping.Models;
using Mapster;

namespace Fake.ObjectMapping.Mapster.Profiles;

public class MappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<TestEntity, TestDto>()
            .Map(dest => dest.CreateTime, src => src.CreateTime.ToString("yyyy-MM-dd"));
    }
}