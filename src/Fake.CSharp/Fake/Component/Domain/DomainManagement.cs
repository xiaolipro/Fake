using System.Collections.Concurrent;
using Fake.CSharp.Fake.Domain;

namespace Fake.CSharp;

public class DomainManagement
{
    private readonly ConcurrentDictionary<string, WeakReference> Cache;

    public DomainManagement()
    {
        Cache = new();
    }

    public FakeReferenceDomain Random()
    {
        return Create($"N{Guid.NewGuid().ToString($"N")}");
    }

    private FakeReferenceDomain Create(string key)
    {
        if (Cache.ContainsKey(key))
        {
            return (FakeReferenceDomain)(Cache[key].Target);
        }
        else
        {
            Clear();
            var domain = new FakeReferenceDomain(key);
            Add(key, domain);
            return domain;
        }
    }

    private void Add(string key, FakeReferenceDomain domain)
    {
        if (Cache.ContainsKey(key))
        {
            if (!Cache[key].IsAlive)
            {
                Cache[key] = new WeakReference(domain);
            }
        }
        else
        {
            Cache[key] = new WeakReference(domain, trackResurrection: true);
        }
    }

    private void Clear()
    {
        foreach (var item in Cache)
        {
            if (!item.Value.IsAlive)
            {
                Cache.TryRemove(item.Key, out _);
            }
        }
    }

    public WeakReference Remove(string key)
    {
        if (Cache.ContainsKey(key))
        {
            Cache.TryRemove(key, out var result);

            if (result != default)
            {
                ((FakeReferenceDomain)result.Target)?.Dispose();
            }

            return result;
        }

        throw new Exception($"找不到 key : {key}");
    }
    
    public bool IsDeleted(string key)
    {
        if(Cache.TryGetValue(key, out var value))
        {
            return value.IsAlive;
        }
        
        return true;
    }
    
    public FakeReferenceDomain? Get(string key)
    {
        if(Cache.ContainsKey(key))
        {
            return (FakeReferenceDomain?)Cache[key].Target;
        }
        
        return null;
    }
}

public sealed class FakeReferenceDomain : FakeDomain
{
    public readonly FakeReferenceDomain DefaultDomain;

    public FakeReferenceDomain(DomainManagement domainManagement)
    {
        DefaultDomain = new FakeReferenceDomain();
    }
}
