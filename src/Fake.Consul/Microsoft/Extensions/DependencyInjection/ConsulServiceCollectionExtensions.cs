using System;
using Consul;
using Fake;
using Fake.Consul;
using Fake.Consul.LoadBalancing;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConsulServiceCollectionExtensions
{
    /// <summary>
    /// 添加Consul
    /// </summary>
    /// <param name="services"></param>
    public static void AddConsul(this IServiceCollection services)
    {
        var consulClientOptions = services.GetInstance<IOptions<FakeConsulServiceOptions>>().Value;
        services.AddSingleton<IConsulClient, ConsulClient>(_ => new ConsulClient(consulConfig =>
        {
            consulConfig.Address = consulClientOptions.Address;
            consulConfig.Datacenter = consulClientOptions.Datacenter;
            consulConfig.Token = consulClientOptions.Token;
        }));
        services.AddSingleton<IHostedService, ConsulHostedService>();
    }
}