using System.Collections.Concurrent;
using System.Collections.Generic;
using Consul;
using Microsoft.Extensions.Logging;

namespace Fake.Consul.LoadBalancing;

public class ConsulPollingServiceBalancer : AbstractConsulServiceBalancer
{
    public ConsulPollingServiceBalancer(ILogger<ConsulPollingServiceBalancer> logger, IConsulClient client)
        : base(logger, client)
    {
    }

    protected readonly ConcurrentDictionary<string, ulong> ServiceCounters = new();
    
    protected override int PickIndex(string serviceName)
    {
        ServiceCounters.GetOrAdd(serviceName, 0);
        return (int)(ServiceCounters[serviceName]++ % (ulong)ServiceNodes.Count);
    }

    protected override void OnServiceEntry(string serviceName, List<ServiceEntry> entries)
    {
        ServiceCounters.TryAdd(serviceName, 0);
    }
}