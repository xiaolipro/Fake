using System;

namespace Fake.Localization.Contributors;

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