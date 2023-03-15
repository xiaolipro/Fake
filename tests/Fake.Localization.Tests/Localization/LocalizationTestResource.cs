using Fake.Localization;
using Fake.Modularity;

namespace Localization;

[DependsOn(typeof(LocalizationTestValidationResource))]
[LocalizationResourceName("TestResource")]
public sealed class LocalizationTestResource
{
    
}