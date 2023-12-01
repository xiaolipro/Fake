namespace Fake.Localization;

public class NonTypedLocalizationResource : AbstractLocalizationResource
{
    public NonTypedLocalizationResource(string? resourceName, string? defaultCultureName = null) :
        base(resourceName, defaultCultureName)
    {
    }
}