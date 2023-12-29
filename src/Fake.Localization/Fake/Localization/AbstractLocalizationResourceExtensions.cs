using System;
using System.Collections.Generic;
using Fake.Localization.Contributors;

namespace Fake.Localization;

public static class LocalizationResourceBaseExtensions
{
    public static TLocalizationResource AddDepends<TLocalizationResource>(
        this TLocalizationResource localizationResource, params Type[] types)
        where TLocalizationResource : LocalizationResourceBase
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
        where TLocalizationResource : LocalizationResourceBase
    {
        ThrowHelper.ThrowIfNull(localizationResource, nameof(localizationResource));
        ThrowHelper.ThrowIfNull(virtualPath, nameof(virtualPath));

        localizationResource.Contributors.Add(new JsonVirtualLocalizationResourceContributorBase(virtualPath));

        return localizationResource;
    }
}