using JetBrains.Annotations;

namespace Fake.Localization;

public class NonTypedLocalizationResource : AbstractLocalizationResource
{
    public NonTypedLocalizationResource([NotNull] string resourceName, [CanBeNull] string defaultCultureName = null) :
        base(resourceName, defaultCultureName)
    {
    }
}