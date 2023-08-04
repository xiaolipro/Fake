using System.Collections.Concurrent;

namespace Fake.CSharp;

public class DomainManagement
{
    private readonly ConcurrentDictionary<string , WeakReference> Cache;
    public DomainManagement()
    {
        Cache = new();
    }
}
