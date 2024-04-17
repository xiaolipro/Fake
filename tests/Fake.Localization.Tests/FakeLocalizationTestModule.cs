using Fake.Localization.Tests.Localization;
using Fake.Modularity;
using Fake.Testing;
using Fake.VirtualFileSystem;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Localization.Tests;

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
            options.Resources.Add<LocalizationTestResource>("zh")
                .LoadVirtualJson("/Resources");

            options.Resources.Add("LocalizationTestCountryNames", "zh")
                .LoadVirtualJson("/Resources/CountryNames");

            options.Resources.Add<LocalizationTestValidationResource>("zh")
                .LoadVirtualJson("/Resources/Validation");
        });
    }
}