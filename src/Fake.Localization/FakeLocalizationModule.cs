using Fake.Localization;
using Fake.Modularity;
using Fake.VirtualFileSystem;
using Microsoft.Extensions.DependencyInjection;


[DependsOn(
    typeof(FakeVirtualFileSystemModule)
)]
public class FakeLocalizationModule:FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<FakeVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<FakeLocalizationModule>("Fake", "Fake");
        });
        
        context.Services.Configure<FakeLocalizationOptions>(options =>
        {
            options.Resources.Add<DefaultResource>("en")
        })
    }
}