using System;
using System.Collections.Generic;
using System.Linq;
using Fake.Modularity;

namespace Fake.Localization;

public class LocalizationResource : LocalizationResourceBase
{
    public Type ResourceType { get; }

    public LocalizationResource(
        Type resourceType,
        string? defaultCultureName = null)
        : base(
            LocalizationResourceNameAttribute.GetName(resourceType),
            defaultCultureName)
    {
        ThrowHelper.ThrowIfNull(resourceType, nameof(resourceType));
        ResourceType = resourceType;
        AddBaseResourceTypes();
    }

    protected virtual void AddBaseResourceTypes()
    {
        var descriptors = ResourceType
            .GetCustomAttributes(true)
            .OfType<IDependsOnProvider>();

        foreach (var descriptor in descriptors)
        {
            foreach (var baseResourceType in descriptor.GetDependedTypes())
            {
                BaseResourceNames.TryAdd(LocalizationResourceNameAttribute.GetName(baseResourceType));
            }
        }
    }
}