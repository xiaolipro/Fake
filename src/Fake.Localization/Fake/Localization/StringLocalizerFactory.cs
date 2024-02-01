using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Fake.Helpers;
using Fake.Localization.Contributors;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Fake.Localization;

public class StringLocalizerFactory(
    IOptions<FakeLocalizationOptions> options,
    ResourceManagerStringLocalizerFactory innerFactory,
    IServiceProvider serviceProvider)
    : IFakeStringLocalizerFactory
{
    private readonly FakeLocalizationOptions _options = options.Value;

    private readonly Dictionary<string, IStringLocalizer> _localizerCache = new();

    // 工厂模式，巧用锁存
    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    public IStringLocalizer Create(Type resourceType)
    {
        var resource = _options.Resources.GetOrDefault(resourceType);

        if (resource == null) return innerFactory.Create(resourceType);

        return CreateStringLocalizer(resource, true);
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        return innerFactory.Create(baseName, location);
    }

    public IStringLocalizer? CreateDefaultOrNull()
    {
        if (_options.DefaultResourceType == null)
        {
            return null;
        }

        return Create(_options.DefaultResourceType);
    }

    public IStringLocalizer? CreateByResourceNameOrNull(string resourceName)
    {
        var resource = _options.Resources.GetOrDefault(resourceName);

        if (resource == null) return null;
        return CreateStringLocalizer(resource, true);
    }

    private IStringLocalizer CreateStringLocalizer(LocalizationResourceBase resource, bool latched)
    {
        string resourceResourceName = resource.ResourceName;

        if (_localizerCache.TryGetValue(resourceResourceName, out var localizer))
        {
            return localizer;
        }

        if (!latched)
        {
            return _localizerCache.GetOrAdd(resourceResourceName, _ => CreateStringLocalizer(resource));
        }

        using (Semaphore.Lock())
        {
            // DCL
            if (_localizerCache.TryGetValue(resourceResourceName, out localizer))
            {
                return localizer;
            }

            return _localizerCache.GetOrAdd(resourceResourceName, _ => CreateStringLocalizer(resource));
        }
    }

    private IStringLocalizer CreateStringLocalizer(LocalizationResourceBase resource)
    {
        foreach (var contributor in _options.GlobalContributors)
        {
            resource.Contributors.Add(ReflectionHelper.CreateInstance<ILocalizationResourceContributor>(contributor));
        }

        var context = new LocalizationResourceInitializationContext(resource, serviceProvider);

        foreach (var contributor in resource.Contributors)
        {
            contributor.Initialize(context);
        }

        var inheritsLocalizer = resource.BaseResourceNames
            .Select(baseResourceName =>
            {
                var baseResource = _options.Resources.GetOrDefault(baseResourceName);
                if (baseResource == null) return null;
                return CreateStringLocalizer(baseResource, false);
            })
            .Where(x => x != null)
            .ToList();

        return new InMemoryStringLocalizer(resource, inheritsLocalizer!, _options);
    }
}