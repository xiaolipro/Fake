using AutoMapper;
using Fake.ObjectMapping.Models;

namespace Fake.ObjectMapping.AutoMapper.Profiles;

public class MyProfile : Profile
{
    public MyProfile()
    {
        CreateMap<MyEntity, MyDto>().ReverseMap();
        CreateMap<MyEntity, SimpleEntity>().ReverseMap();

        CreateMap<SimpleEntity, MyMoreValidateDto>().ReverseMap();
    }
}