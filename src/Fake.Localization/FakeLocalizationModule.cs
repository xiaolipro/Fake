using Fake.Localization;
using Fake.Localization.Resources;
using Fake.Modularity;
using Fake.VirtualFileSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

[DependsOn(
    typeof(FakeVirtualFileSystemModule)
)]
public class FakeLocalizationModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));

        // 替换ASPNETCORE原生的IStringLocalizerFactory实现
        context.Services.Replace(ServiceDescriptor.Singleton<IStringLocalizerFactory, FakeStringLocalizerFactory>());
        context.Services.AddSingleton<ResourceManagerStringLocalizerFactory>();

        context.Services.Configure<FakeVirtualFileSystemOptions>(options =>
        {
            options.FileProviders.Add<FakeLocalizationModule>("Fake");
        });

        context.Services.Configure<FakeLocalizationOptions>(options =>
        {
            options.Resources.Add<FakeLocalizationResource>("zh")
                .LoadVirtualJson("/Localization/Resources");
        });
    }
}