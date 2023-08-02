using Fake.ObjectMapping.Models.Dtos;
using Fake.ObjectMapping.Models.Entities;
using Mapster;

namespace Fake.ObjectMapping.Mapster.Registers;

public class MappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<TestEntity, TestDto>()
            .Map(dest => dest.CreateTime, src => src.CreateTime.ToString("yyyy-MM-dd"));

        config.NewConfig<SourceEntity, SourceDto>();
    }
}