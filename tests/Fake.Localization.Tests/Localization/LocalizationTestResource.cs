using Fake.Localization;
using Fake.Localization.Resources;
using Fake.Modularity;

namespace Localization;

[DependsOn(typeof(LocalizationTestValidationResource))]
[DependsOn(typeof(FakeLocalizationResource))]
[LocalizationResourceName("TestResource")]
public sealed class LocalizationTestResource
{
    
}