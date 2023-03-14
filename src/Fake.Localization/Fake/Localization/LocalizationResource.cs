using System;
using System.Collections.Generic;
using System.Linq;
using Fake.Modularity;
using JetBrains.Annotations;

namespace Fake.Localization;

public class LocalizationResource : AbstractLocalizationResource
{
    [NotNull] public Type ResourceType { get; }

    public LocalizationResource(
        [NotNull] Type resourceType,
        [CanBeNull] string defaultCultureName = null)
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