using System;
using System.Collections.Generic;

namespace Fake.Localization;

/// <summary>
/// 本地化资源字典 资源名称：资源
/// </summary>
public class LocalizationResourceDictionary : Dictionary<string, AbstractLocalizationResource>
{
    private readonly Dictionary<Type, AbstractLocalizationResource?> _resourcesByTypes = new();

    public LocalizationResource? Add<TLocalizationResource>(string? defaultCultureName = null)
    {
        return Add(typeof(TLocalizationResource), defaultCultureName);
    }

    public LocalizationResource? Add(Type resourceType, string? defaultCultureName = null)
    {
        var resourceName = LocalizationResourceNameAttribute.GetName(resourceType);
        if (ContainsKey(resourceName))
        {
            throw new FakeException("该本地化资源已存在: " + resourceType.AssemblyQualifiedName);
        }

        var resource = new LocalizationResource(resourceType, defaultCultureName);

        this[resourceName] = resource;
        _resourcesByTypes[resourceType] = resource;

        return resource;
    }

    public NonTypedLocalizationResource Add(string? resourceName, string? defaultCultureName = null)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(resourceName, nameof(resourceName));

        if (ContainsKey(resourceName))
        {
            throw new FakeException("该本地化资源已存在: " + resourceName);
        }

        var resource = new NonTypedLocalizationResource(resourceName, defaultCultureName);

        this[resourceName] = resource;

        return resource;
    }

    public AbstractLocalizationResource Get<TResource>()
    {
        var resourceType = typeof(TResource);

        var resource = _resourcesByTypes.GetOrDefault(resourceType);
        if (resource == null)
        {
            throw new FakeException("找不到给定类型的本地化资源：" + resourceType.AssemblyQualifiedName);
        }

        return resource;
    }

    public AbstractLocalizationResource Get(string resourceName)
    {
        var resource = this.GetOrDefault(resourceName);
        if (resource == null)
        {
            throw new FakeException("找不到给定名称的本地化资源: " + resourceName);
        }

        return resource;
    }

    public AbstractLocalizationResource? GetOrDefault(Type resourceType)
    {
        return _resourcesByTypes.GetOrDefault(resourceType);
    }
}