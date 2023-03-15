using Fake.Localization;
using Fake.Modularity;
using Fake.VirtualFileSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;


[DependsOn(
    typeof(FakeVirtualFileSystemModule)
)]
public class FakeLocalizationModule:FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 替换ASPNETCORE原生的IStringLocalizerFactory实现
        context.Services.Replace(ServiceDescriptor.Singleton<IStringLocalizerFactory, FakeStringLocalizerFactory>());

        context.Services.AddSingleton<VirtualFileProvider>();
        
        context.Services.Configure<FakeVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<FakeLocalizationModule>("Fake", "Fake");
        });

        context.Services.Configure<FakeLocalizationOptions>(options =>
        {
            options.Resources.Add<DefaultResource>("en");
        });
    }
}