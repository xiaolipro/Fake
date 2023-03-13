using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Fake.Localization.VirtualFile;

public class JsonLocalizationResourceContributor:ILocalizationResourceContributor
{
    public bool IsDynamic { get; }
    public void Initialize(LocalizationResourceInitializationContext context)
    {
        throw new System.NotImplementedException();
    }

    public LocalizedString GetOrNull(string cultureName, string name)
    {
        throw new System.NotImplementedException();
    }

    public void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        throw new System.NotImplementedException();
    }

    public Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<string>> GetSupportedCulturesAsync()
    {
        throw new System.NotImplementedException();
    }
}