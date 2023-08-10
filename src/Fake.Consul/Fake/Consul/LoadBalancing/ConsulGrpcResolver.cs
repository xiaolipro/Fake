using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Fake.AspNetCore.LoadBalancing;
using Fake.Data;
using Microsoft.Extensions.Logging;

namespace Fake.Consul.LoadBalancing
{
    public class ConsulGrpcResolver : ILoadBalancer
    {
        private readonly ILogger<ConsulGrpcResolver> _logger;
        private readonly IConsulClient _client;

        public ConsulGrpcResolver(ILogger<ConsulGrpcResolver> logger, IConsulClient client)
        {
            _logger = logger;
            _client = client;
        }

        public string Name { get; } = nameof(ConsulGrpcResolver);
        public TimeSpan RefreshInterval { get; } = TimeSpan.FromSeconds(15);

        public async Task<(List<Uri> serviceUris, dynamic metaData)> ResolutionService(string serviceName)
        {
            var entries = await _client.Health.Service(serviceName);

            var uris = entries.Response
                .Select(x => new Uri($"http://{x.Service.Address}:{x.Service.Meta["GrpcPort"]}")).ToList();

            if (uris.Count < 1)
            {
                _logger.LogWarning("未能从服务：{ServiceName} 中解析到任何实例，耗时：{RequestTimeTotalMilliseconds}ms", serviceName,
                    entries.RequestTime.TotalMilliseconds);
            }
            else
            {
                _logger.LogInformation(
                    "解析服务：{ServiceName} 成功：{Uris}，耗时：{RequestTimeTotalMilliseconds}ms", serviceName,
                    string.Join(",", uris), entries.RequestTime.TotalMilliseconds);
            }

            return (uris, entries.Response.Select(entry => entry.Service.Meta as dynamic).ToList());
        }

        public ExtraPropertyDictionary ExtraProperties { get; }
        public List<string> ServiceAddresses { get; }
        public async Task ResolutionAsync(string serviceName)
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

            ServiceAddresses = addresses;
        }

        public string Balancing()
        {
            throw new NotImplementedException();
        }
    }
}