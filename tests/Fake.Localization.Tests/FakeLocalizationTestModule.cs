using Fake.Localization;
using Fake.Modularity;
using Fake.Testing;
using Fake.VirtualFileSystem;
using Localization;
using Microsoft.Extensions.DependencyInjection;

[DependsOn(typeof(FakeTestingModule))]
[DependsOn(typeof(FakeLocalizationModule))]
public class FakeLocalizationTestModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<FakeVirtualFileSystemOptions>(options =>
        {
            options.FileProviders.Add<FakeLocalizationTestModule>("Localization");
        });

        context.Services.Configure<FakeLocalizationOptions>(options =>
        {
            options.TryGetFromParentCulture = true;
            options.TryGetFromDefaultCulture = true;

            options.Resources.Add<LocalizationTestResource>("zh")
                .LoadVirtualJson("/Resources");

            options.Resources.Add("LocalizationTestCountryNames", "zh")
                .LoadVirtualJson("/Resources/CountryNames");

            options.Resources.Add<LocalizationTestValidationResource>("zh")
                .LoadVirtualJson("/Resources/Validation");
        });
    }
}