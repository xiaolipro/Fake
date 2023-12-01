using JetBrains.Annotations;

namespace Fake.Localization;

public class NonTypedLocalizationResource : AbstractLocalizationResource
{
    public NonTypedLocalizationResource(string resourceName, [CanBeNull] string defaultCultureName = null) :
        base(resourceName, defaultCultureName)
    {
    }
}