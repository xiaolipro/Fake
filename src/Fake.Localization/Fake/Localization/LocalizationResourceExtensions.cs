using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Fake.Localization;

public static class LocalizationResourceExtensions
{
    public static TLocalizationResource AddDepends<TLocalizationResource>(
        [NotNull] this TLocalizationResource localizationResource, [NotNull] params Type[] types)
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

    public static TLocalizationResource AddJsonFile<TLocalizationResource>(
        [NotNull] this TLocalizationResource localizationResource, [NotNull] string virtualPath)
        where TLocalizationResource : AbstractLocalizationResource
    {
        ThrowHelper.ThrowIfNull(localizationResource, nameof(localizationResource));
        ThrowHelper.ThrowIfNull(virtualPath, nameof(virtualPath));
        
        localizationResource
    }
}