using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Fake.Localization.Contributors;
using Fake.Reflection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Fake.Localization;

public class FakeStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly ResourceManagerStringLocalizerFactory _innerFactory;
    private readonly FakeLocalizationOptions _options;
    private readonly Dictionary<string, IStringLocalizer> _localizerCache;
    // 工厂模式，巧用锁存
    private readonly SemaphoreSlim _semaphore;

    public FakeStringLocalizerFactory(IOptions<FakeLocalizationOptions> options,
        ResourceManagerStringLocalizerFactory innerFactory)
    {
        _innerFactory = innerFactory;
        _options = options.Value;
        _localizerCache = new Dictionary<string, IStringLocalizer>();
        _semaphore = new SemaphoreSlim(1, 1);
    }

    public IStringLocalizer Create(Type resourceSource)
    {
        var resource = _options.Resources.GetOrDefault(resourceSource);

        if (resource == null) return _innerFactory.Create(resourceSource);

        return CreateStringLocalizer(resource, true);
    }


    private IStringLocalizer CreateStringLocalizer(AbstractLocalizationResource resource, bool latched)
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

        using (_semaphore.Begin())
        {
            // DCL
            if (_localizerCache.TryGetValue(resourceResourceName, out localizer))
            {
                return localizer;
            }

            return _localizerCache.GetOrAdd(resourceResourceName, _ => CreateStringLocalizer(resource));
        }
    }

    private IStringLocalizer CreateStringLocalizer(AbstractLocalizationResource resource)
    {
        foreach (var contributor in _options.GlobalContributors)
        {
            resource.Contributors.Add(ReflectionHelper.CreateInstance<ILocalizationResourceContributor>(contributor));
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
        
        return new InMemoryStringLocalizer(resource, inheritsLocalizer, _options);
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        throw new NotImplementedException();
    }
}