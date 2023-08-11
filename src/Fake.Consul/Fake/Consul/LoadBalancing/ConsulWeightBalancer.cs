using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Fake.Helpers;
using Fake.LoadBalancing;
using Microsoft.Extensions.Logging;

namespace Fake.Consul.LoadBalancing;

public class ConsulWeightBalancer: IFakeBalancer
{
    private readonly ILogger<ConsulWeightBalancer> _logger;
    private readonly IConsulClient _client;
    public ConsulWeightBalancer(ILogger<ConsulWeightBalancer> logger, IConsulClient client)
    {
        _logger = logger;
        _client = client;
    }

    public ConcurrentDictionary<string, List<string>> ServiceNodes { get; } = new();
    public ConcurrentDictionary<string, List<int>> ServiceNodeWeights { get; } = new();

    public async Task ReferenceAsync(string serviceName)
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

    private async Task<List<string>> ResolutionAsync(string serviceName)
    {
        var entries = await _client.Health.Service(serviceName);

        var nodes = entries.Response.Select(x => x.Service.Address).ToList();
        var weights = entries.Response.Select(x =>
        {
            var isOk = int.TryParse(x.Service.Meta[nameof(FakeConsulClientOptions.Weight)], out var weight);
            return isOk ? weight : 0;
        }).ToList();

        ServiceNodeWeights.TryAdd(serviceName, weights);

        if (ServiceNodes.Count < 1)
        {
            _logger.LogWarning("未能从服务：{ServiceName} 中解析到任何实例，耗时：{RequestTimeTotalMilliseconds}ms", serviceName,
                entries.RequestTime.TotalMilliseconds);
        }
        else
        {
            _logger.LogInformation(
                "解析服务：{ServiceName} 成功：{Uris}，耗时：{RequestTimeTotalMilliseconds}ms", serviceName,
                string.Join(", ", ServiceNodes), entries.RequestTime.TotalMilliseconds);
        }

        return nodes;
    }

    public string Pick(string serviceName)
    {
        var nodes = ServiceNodes[serviceName];
        if (nodes == null || nodes.Count == 0)
        {
            throw new FakeException($"服务节点列表为空，服务名称：{serviceName}");
        }
        
        if (nodes.Count == 1)
        {
            return nodes[0];
        }

        var serviceNodeWeightSum = ServiceNodeWeights[serviceName].Sum();
        var serviceNodeWeightRandom = RandomHelper.Next(0, serviceNodeWeightSum);
        var serviceNodeWeightSumTemp = 0;
        for (var i = 0; i < nodes.Count; i++)
        {
            serviceNodeWeightSumTemp += ServiceNodeWeights[serviceName][i];
            if (serviceNodeWeightSumTemp >= serviceNodeWeightRandom)
            {
                return nodes[i];
            }
        }
        
        return nodes[0];
    }
}