using System;
using System.Collections.Generic;
using Fake.Localization.Contributors;

namespace Fake.Localization;

public static class AbstractLocalizationResourceExtensions
{
    public static TLocalizationResource AddDepends<TLocalizationResource>(
        this TLocalizationResource localizationResource, params Type[] types)
        where TLocalizationResource : AbstractLocalizationResource
    {
        ThrowHelper.ThrowIfNull(localizationResource, nameof(localizationResource));
        ThrowHelper.ThrowIfNull(types, nameof(types));

        foreach (var type in types)
        {
            localizationResource.BaseResourceNames
                .TryAdd(LocalizationResourceNameAttribute.GetName(type));
        }

        return localizationResource;
    }

    public static TLocalizationResource LoadVirtualJson<TLocalizationResource>(
        this TLocalizationResource localizationResource, string virtualPath)
        where TLocalizationResource : AbstractLocalizationResource
    {
        ThrowHelper.ThrowIfNull(localizationResource, nameof(localizationResource));
        ThrowHelper.ThrowIfNull(virtualPath, nameof(virtualPath));

        localizationResource.Contributors.Add(new JsonVirtualLocalizationResourceContributor(virtualPath));

        return localizationResource;
    }
}