using System.Collections.Generic;
using JetBrains.Annotations;

namespace Fake.Localization;

public abstract class AbstractLocalizationResource
{
    [NotNull] 
    public string ResourceName { get; }
    [CanBeNull]
    public string DefaultCultureName { get; set; }
    public List<string> BaseResourceNames { get; }
    
    [NotNull]
    public LocalizationResourceContributorList Contributors { get; }
    public AbstractLocalizationResource(
        [NotNull] string resourceName,
        [CanBeNull] string defaultCultureName = null)
    {
        ThrowHelper.ThrowIfNull(resourceName, nameof(resourceName));
        ResourceName = resourceName;
        DefaultCultureName = defaultCultureName;
        
        BaseResourceNames = new();
    }
}