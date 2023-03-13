using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Fake.Localization;

public interface ILocalizationResourceContributor
{
    void Initialize(LocalizationResourceInitializationContext context);
    
    LocalizedString GetOrNull(string cultureName, string name);

    void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary);

    Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary);

    Task<IEnumerable<string>> GetSupportedCulturesAsync();
}


public class LocalizationResourceInitializationContext
{
    public AbstractLocalizationResource Resource { get; }
    
    public IServiceProvider ServiceProvider { get; }
    
    public LocalizationResourceInitializationContext(AbstractLocalizationResource resource, IServiceProvider serviceProvider)
    {
        Resource = resource;
        ServiceProvider = serviceProvider;
    }
}