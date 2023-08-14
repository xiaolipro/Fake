using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Consul;
using Fake.Helpers;
using Microsoft.Extensions.Logging;

namespace Fake.Consul.LoadBalancing;

public class ConsulWeightServiceBalancer: AbstractConsulServiceBalancer
{
    public ConsulWeightServiceBalancer(ILogger<ConsulWeightServiceBalancer> logger, IConsulClient client) : base(logger, client)
    {
    }

    protected ConcurrentDictionary<string, List<int>> ServiceNodeWeights { get; } = new();

    protected override int PickIndex(string serviceName)
    {
        var nodes = ServiceNodes[serviceName];

        if (nodes.Count == 1) return 0;
        
        var serviceNodeWeightSum = ServiceNodeWeights[serviceName].Sum();
        var serviceNodeWeightRandom = RandomHelper.Next(0, serviceNodeWeightSum);
        var serviceNodeWeightSumTemp = 0;
        
        for (var idx = 0; idx < nodes.Count; idx++)
        {
            serviceNodeWeightSumTemp += ServiceNodeWeights[serviceName][idx];
            if (serviceNodeWeightSumTemp >= serviceNodeWeightRandom)
            {
                return idx;
            }
        }

        return 0;
    }

    protected override void OnServiceEntry(string serviceName, List<ServiceEntry> entries)
    {
        var weights = entries.Select(x =>
            int.TryParse(x.Service.Meta[nameof(FakeConsulClientOptions.Weight)], out var weight) ? weight : 0).ToList();
        ServiceNodeWeights.TryAdd(serviceName, weights);
    }
}