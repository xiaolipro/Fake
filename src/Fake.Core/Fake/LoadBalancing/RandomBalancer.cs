using System.Collections.Concurrent;
using Fake.Helpers;

namespace Fake.LoadBalancing;

public abstract class RandomBalancer:IFakeBalancer
{
    public ConcurrentDictionary<string, List<string>> ServiceNodes { get; } = new();

    public virtual async Task ReferenceAsync(string serviceName)
    {
        var nodes = await ResolutionAsync(serviceName);
        if (ServiceNodes.TryGetValue(serviceName, out var oldNodes))
        {
            oldNodes.Clear();
            oldNodes.AddRange(nodes);
        }
        else
        {
            ServiceNodes.TryAdd(serviceName, nodes);
        }
    }
    
    public abstract Task<List<string>> ResolutionAsync(string serviceName);

    public virtual string Pick(string serviceName)
    {
        if (ServiceNodes[serviceName] == null || ServiceNodes[serviceName].Count == 0)
        {
            throw new FakeException("服务地址集合为空");
        }
        
        return ServiceNodes[serviceName][RandomHelper.Next(0, ServiceNodes[serviceName].Count)];
    }
}