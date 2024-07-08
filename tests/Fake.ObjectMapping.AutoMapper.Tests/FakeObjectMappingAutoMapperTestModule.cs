using Fake.Modularity;
using Fake.ObjectMapping.AutoMapper.Tests.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.ObjectMapping.AutoMapper.Tests;

[DependsOn(typeof(FakeObjectMappingAutoMapperModule))]
public class FakeObjectMappingAutoMapperTestModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<FakeAutoMapperOptions>(options =>
        {
            options.ScanProfiles<FakeObjectMappingAutoMapperTestModule>(validate: true);
            options.ValidatingProfiles.Remove<NoValidateProfile>();
        });
    }
}