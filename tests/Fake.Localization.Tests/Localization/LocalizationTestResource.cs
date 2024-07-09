using Fake.Modularity;

namespace Fake.Localization.Tests.Localization;

[DependsOn(typeof(LocalizationTestValidationResource))]
[DependsOn(typeof(FakeLocalizationResource))]
[LocalizationResourceName("TestResource")]
public sealed class LocalizationTestResource
{
}