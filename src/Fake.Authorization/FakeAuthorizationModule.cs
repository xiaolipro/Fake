using Fake.Authorization.Localization;
using Fake.Localization;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[DependsOn(typeof(FakeLocalizationModule))]
public class FakeAuthorizationModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAuthorizationCore();

        context.Services.Configure<FakeVirtualFileSystemOptions>(options =>
        {
            options.FileProviders.Add<FakeAuthorizationModule>("Fake/Authorization");
        });

        context.Services.Configure<FakeLocalizationOptions>(options =>
        {
            options.Resources
                .Add<FakeAuthorizationResource>("zh")
                .LoadVirtualJson("/Localization");
        });
    }
}