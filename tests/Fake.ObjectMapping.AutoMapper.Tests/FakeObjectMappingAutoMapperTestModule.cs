using Fake.Modularity;
using Fake.ObjectMapping.AutoMapper;
using Fake.ObjectMapping.AutoMapper.Profiles;
using Microsoft.Extensions.DependencyInjection;


[DependsOn(typeof(FakeObjectMappingAutoMapperModule))]
public class FakeObjectMappingAutoMapperTestModule:FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<FakeAutoMapperOptions>(options =>
        {
            options.AddMaps<FakeObjectMappingAutoMapperTestModule>(validate: true);
            options.ValidatingProfile.Remove<NoValidateProfile>();
        });
    }
}