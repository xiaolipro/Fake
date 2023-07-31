using Fake.ObjectMapping.Models;
using Microsoft.Extensions.Options;

namespace Fake.ObjectMapping.Mapster.Profiles;

public class MyProfile : Profile
{
    public MyProfile(IOptions<FakeMapsterOptions> options) : base(options)
    {
        CreateForType<TestEntity, TestDto>()
            .Map(dest => dest.CreateTime, src => src.CreateTime.ToString("yyyy-MM-dd"));
    }
}