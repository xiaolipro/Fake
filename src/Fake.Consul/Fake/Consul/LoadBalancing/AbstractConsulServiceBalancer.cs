using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Fake.LoadBalancing;
using Microsoft.Extensions.Logging;

namespace Fake.Consul.LoadBalancing;

public abstract class AbstractConsulServiceBalancer : IServiceBalancer
{
    public ConcurrentDictionary<string, List<ServiceResolveResult>> ServiceNodes { get; } = new();

    private readonly ILogger<AbstractConsulServiceBalancer> _logger;
    private readonly IConsulClient _client;

    public AbstractConsulServiceBalancer(ILogger<AbstractConsulServiceBalancer> logger, IConsulClient client)
    {
        _logger = logger;
        _client = client;
    }

    public virtual async Task ResolutionAsync(string serviceName)
    {
        var nodes = await GetBalancerResolveResultsAsync(serviceName);
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

    public virtual string Pick(string serviceName, bool isGrpc = false)
    {
        if (ServiceNodes == null || !ServiceNodes.TryGetValue(serviceName, out var nodes) || nodes.Count == 0)
        {
            throw new FakeException($"服务：{serviceName} 节点集合为空，无法pick");
        }

        var idx = PickIndex(serviceName);
        if (idx >= nodes.Count || idx < 0)
        {
            idx = 0;
            _logger.LogWarning("均衡服务：{ServiceName} pick的索引：{Index} 超出范围，已重置为 0", serviceName, idx);
        }
        var res = nodes[idx];
        return $"{res.Host}:{(isGrpc ? res.GrpcPort : res.Port)}";
    }
    
    public virtual async Task<List<ServiceResolveResult>> GetBalancerResolveResultsAsync(string serviceName)
    {
        var entries = await _client.Health.Service(serviceName);

        var nodes = entries.Response.ToList();

        OnServiceEntry(serviceName, nodes);

        if (nodes.Count < 1)
        {
            _logger.LogWarning("未能从服务：{ServiceName} 中解析到任何实例，耗时：{RequestTimeTotalMilliseconds}ms", serviceName,
                entries.RequestTime.TotalMilliseconds);
        }
        else
        {
            _logger.LogInformation(
                "解析服务：{ServiceName} 成功：{Uris}，耗时：{RequestTimeTotalMilliseconds}ms", serviceName,
                string.Join(", ", nodes), entries.RequestTime.TotalMilliseconds);
        }

        return nodes.Select(x => new ServiceResolveResult
        {
            Host = x.Service.Address,
            Port = x.Service.Port,
            GrpcPort = int.TryParse(x.Service.Meta[nameof(FakeConsulRegisterOptions.Weight)], out var weight) ? weight : 0
        }).ToList();
    }
    
    protected abstract int PickIndex(string serviceName);

    protected virtual void OnServiceEntry(string serviceName, List<ServiceEntry> entries)
    {
    }
}