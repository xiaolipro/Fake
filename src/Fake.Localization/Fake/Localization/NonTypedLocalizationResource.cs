namespace Fake.Localization;

public class NonTypedLocalizationResource(string resourceName, string? defaultCultureName = null)
    : LocalizationResourceBase(resourceName, defaultCultureName);