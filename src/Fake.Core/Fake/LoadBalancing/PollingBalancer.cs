using System.Collections.Concurrent;

namespace Fake.LoadBalancing;

public abstract class PollingBalancer : IFakeBalancer
{
    protected readonly ConcurrentDictionary<string, object> ServiceLocks = new();
    protected readonly ConcurrentDictionary<string, int> ServiceCounters = new();
    public ConcurrentDictionary<string, List<string>> ServiceNodes { get; } = new();

    public virtual async Task ReferenceAsync(string serviceName)
    {
        ServiceLocks.TryAdd(serviceName, new object());
        ServiceCounters.TryAdd(serviceName, 0);
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
        if (ServiceNodes == null || !ServiceNodes.TryGetValue(serviceName, out var nodes) || nodes.Count == 0)
        {
            throw new FakeException("服务地址集合为空");
        }

        lock (ServiceLocks[serviceName])
        {
            if (ServiceCounters[serviceName] >= nodes.Count)
            {
                ServiceCounters[serviceName] = 0;
            }

            var address = nodes[ServiceCounters[serviceName]];
            ServiceCounters[serviceName]++;
            return address;
        }
    }
}