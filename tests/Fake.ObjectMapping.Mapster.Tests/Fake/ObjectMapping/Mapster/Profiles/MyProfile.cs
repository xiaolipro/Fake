using Fake.ObjectMapping.Models;

namespace Fake.ObjectMapping.Mapster.Profiles;

public class MyProfile : Profile
{
    public MyProfile()
    {
        CreateConfig<TestEntity, TestDto>()
            .Map(dest => dest.CreateTime, src => src.CreateTime.ToString("yyyy-MM-dd"));
    }
}