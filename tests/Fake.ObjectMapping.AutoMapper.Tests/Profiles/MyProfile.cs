using AutoMapper;
using Fake.ObjectMapping.Tests.Models;

namespace Fake.ObjectMapping.AutoMapper.Tests.Profiles;

public class MyProfile : Profile
{
    public MyProfile()
    {
        CreateMap<MyEntity, MyDto>().ReverseMap();
        CreateMap<MyEntity, SimpleEntity>().ReverseMap();

        CreateMap<SimpleEntity, MyMoreValidateDto>().ReverseMap();
    }
}