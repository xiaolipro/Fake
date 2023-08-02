using AutoMapper;
using Fake.ObjectMapping.Models.Entities;

namespace Fake.ObjectMapping.AutoMapper.Profiles;

public class NoValidateProfile : Profile
{
    public NoValidateProfile()
    {
        CreateMap<SimpleEntity, MyMoreNoValidateDto>().ReverseMap();
    }
}