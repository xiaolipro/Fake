using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Extensions.Localization;

namespace Fake.Localization;

public class InMemoryStringLocalizer(
    LocalizationResourceBase resource,
    List<IStringLocalizer> inheritsLocalizer,
    FakeLocalizationOptions options)
    : IFakeStringLocalizer
{
    public virtual LocalizedString this[string name] => GetLocalizedString(name);

    public virtual LocalizedString this[string name, params object[] arguments] =>
        GetLocalizedStringFormatted(name, arguments);

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        return GetAllStrings(CultureInfo.CurrentUICulture.Name, includeParentCultures);
    }

    protected virtual LocalizedString GetLocalizedStringFormatted(string name, params object[] arguments)
    {
        var localizedString = GetLocalizedString(name);
        return new LocalizedString(name, string.Format(localizedString.Value, arguments),
            localizedString.ResourceNotFound, localizedString.SearchedLocation);
    }

    protected virtual LocalizedString GetLocalizedString(string name)
    {
        var cultureName = options.DefaultCulture;
        var localizedString = resource.GetOrNull(cultureName, name);
        if (localizedString != null) return localizedString;

        if (options.TryGetFromParentCulture)
        {
            if (cultureName.Contains("-"))
            {
                localizedString = resource.GetOrNull(CultureHelper.GetParentCultureName(cultureName), name);
                if (localizedString != null) return localizedString;
            }
        }

        if (options.TryGetFromDefaultCulture)
        {
            var defaultCulture = resource.DefaultCultureName;
            if (!defaultCulture.IsNullOrWhiteSpace() && defaultCulture != cultureName)
            {
                localizedString = resource.GetOrNull(defaultCulture!, name);
                if (localizedString != null) return localizedString;
            }
        }

        // 如果还找不到，那就去继承资源找
        foreach (var stringLocalizer in inheritsLocalizer)
        {
            using (CultureHelper.UseCulture(cultureName))
            {
                if (!stringLocalizer[name].ResourceNotFound)
                {
                    return stringLocalizer[name];
                }
            }
        }

        return new LocalizedString(name, name, resourceNotFound: true);
    }


    protected virtual IReadOnlyList<LocalizedString> GetAllStrings(string cultureName,
        bool includeParentCultures = true, bool includeInheritLocalizers = true)
    {
        var allStrings = new Dictionary<string, LocalizedString>();

        // 填充继承的strings
        if (includeInheritLocalizers)
        {
            foreach (var localizer in inheritsLocalizer)
            {
                using (CultureHelper.UseCulture(cultureName))
                {
                    var strings = FakeStringLocalizerExtensions.GetAllStrings(localizer, includeParentCultures);

                    foreach (var localizedString in strings)
                    {
                        allStrings[localizedString.Name] = localizedString;
                    }
                }
            }
        }

        if (includeParentCultures)
        {
            // 填充默认culture strings
            if (!resource.DefaultCultureName.IsNullOrWhiteSpace())
            {
                resource.Fill(resource.DefaultCultureName!, allStrings);
            }

            // 填充parent culture strings
            if (cultureName.Contains("-"))
            {
                resource.Fill(CultureHelper.GetParentCultureName(cultureName), allStrings);
            }
        }

        // 填充自己的strings
        resource.Fill(cultureName, allStrings);

        return allStrings.Values.ToImmutableList();
    }
}