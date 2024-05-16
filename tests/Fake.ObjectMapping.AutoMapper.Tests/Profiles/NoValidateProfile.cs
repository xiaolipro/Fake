using AutoMapper;
using Fake.ObjectMapping.Tests.Models;

namespace Fake.ObjectMapping.AutoMapper.Tests.Profiles;

public class NoValidateProfile : Profile
{
    public NoValidateProfile()
    {
        CreateMap<SimpleEntity, MyMoreNoValidateDto>().ReverseMap();
    }
}