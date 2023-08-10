using System;
using Consul;
using Fake;
using Fake.Consul;
using Fake.Consul.LoadBalancing;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConsulServiceCollectionExtensions
{
    /// <summary>
    /// 添加Consul
    /// </summary>
    /// <param name="services"></param>
    public static void AddConsul(this IServiceCollection services)
    {
        var consulClientOptions = services.GetConfiguration().Get<FakeConsulServiceOptions>();
        services.AddSingleton<IConsulClient, ConsulClient>(_ => new ConsulClient(consulConfig =>
        {
            consulConfig.Address = consulClientOptions.Address;
            consulConfig.Datacenter = consulClientOptions.Datacenter;
        }));
        services.AddSingleton<IHostedService, ConsulHostedService>();
    }
    
    /// <summary>
    /// 添加Consul负载均衡调度器
    /// </summary>
    public static void AddConsulDispatcher(this IServiceCollection services)
    {
        services.TryAddSingleton<IDispatcher, ConsulDispatcher>();
    }
}