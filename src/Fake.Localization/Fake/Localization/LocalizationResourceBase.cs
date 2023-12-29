using System.Collections.Generic;
using System.Linq;
using Fake.Localization.Contributors;
using Microsoft.Extensions.Localization;

namespace Fake.Localization;

public abstract class LocalizationResourceBase
{
    public string ResourceName { get; }

    /// <summary>
    /// 继承的资源
    /// </summary>
    public List<string> BaseResourceNames { get; }

    public string? DefaultCultureName { get; set; }
    public List<ILocalizationResourceContributor> Contributors { get; }

    public LocalizationResourceBase(
        string resourceName,
        string? defaultCultureName = null)
    {
        ThrowHelper.ThrowIfNull(resourceName, nameof(resourceName));
        ResourceName = resourceName;
        DefaultCultureName = defaultCultureName;

        BaseResourceNames = new();
        Contributors = new();
    }

    public void Fill(
        string cultureName,
        Dictionary<string, LocalizedString> dictionary)
    {
        foreach (var contributor in Contributors)
        {
            contributor.Fill(cultureName, dictionary);
        }
    }

    public LocalizedString? GetOrNull(string cultureName, string name)
    {
        // 后者优先
        foreach (var contributor in Contributors.Select(x => x).Reverse())
        {
            var localizedString = contributor.GetOrNull(cultureName, name);

            if (localizedString != null) return localizedString;
        }

        return null;
    }
}