using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Fake.LoadBalancing;
using Microsoft.Extensions.Logging;

namespace Fake.Consul.LoadBalancing;

public class ConsulPollingBalancer : PollingBalancer
{
    private readonly ILogger<ConsulPollingBalancer> _logger;
    private readonly IConsulClient _client;
    public ConsulPollingBalancer(ILogger<ConsulPollingBalancer> logger, IConsulClient client)
    {
        _logger = logger;
        _client = client;
    }
    public override async Task<List<string>> ResolutionAsync(string serviceName)
    {
        var entries = await _client.Health.Service(serviceName);

        var addresses = entries.Response.Select(x =>x.Service.Address).ToList();

        if (addresses.Count < 1)
        {
            _logger.LogWarning("未能从服务：{ServiceName} 中解析到任何实例，耗时：{RequestTimeTotalMilliseconds}ms", serviceName,
                entries.RequestTime.TotalMilliseconds);
        }
        else
        {
            _logger.LogInformation(
                "解析服务：{ServiceName} 成功：{Uris}，耗时：{RequestTimeTotalMilliseconds}ms", serviceName,
                string.Join(", ", addresses), entries.RequestTime.TotalMilliseconds);
        }

        return addresses;
    }
}