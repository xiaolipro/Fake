using System;
using System.Linq;
using JetBrains.Annotations;

namespace Fake.Localization;

public class LocalizationResourceNameAttribute: Attribute
{
    public string Name { get; }

    public LocalizationResourceNameAttribute(string name)
    {
        Name = name;
    }
    
    public static LocalizationResourceNameAttribute GetOrNull(Type resourceType)
    {
        return resourceType
            .GetCustomAttributes(true)
            .OfType<LocalizationResourceNameAttribute>()
            .FirstOrDefault();
    }
    
    [NotNull]
    public static string GetName(Type resourceType)
    {
        return GetOrNull(resourceType)?.Name ?? resourceType.FullName?? resourceType.Name;
    }
}