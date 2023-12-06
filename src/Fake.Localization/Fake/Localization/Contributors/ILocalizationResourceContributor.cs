using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Fake.Localization.Contributors;

public interface ILocalizationResourceContributor
{
    void Initialize(LocalizationResourceInitializationContext context);

    LocalizedString? GetOrNull(string? cultureName, string name);

    void Fill(string? cultureName, Dictionary<string, LocalizedString?> dictionary);

    Task FillAsync(string? cultureName, Dictionary<string, LocalizedString?> dictionary);


    Task<IEnumerable<string?>> GetSupportedCulturesAsync();
}