using System;
using Consul;
using Fake;
using Fake.Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConsulServiceCollectionExtensions
{
    /// <summary>
    /// 添加Consul
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    internal static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
    {
        var consulClientOptions = configuration.Get<FakeConsulClientOptions>();
        ThrowHelper.ThrowIfNull(consulClientOptions, nameof(consulClientOptions), "Consul配置为空");
        services.AddSingleton<IConsulClient, ConsulClient>(_ => new ConsulClient(consulConfig =>
        {
            consulConfig.Address = consulClientOptions.Address;
            consulConfig.Datacenter = consulClientOptions.Datacenter;
            consulConfig.Token = consulClientOptions.Token;
        }));

        return services;
    }

    /// <summary>
    /// 注册consul
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddConsul(this IServiceCollection services)
    {
        if (!services.IsAdded<IConsulClient>())
        {
            throw new FakeInitializationException("请先依赖：" + nameof(FakeConsulModule));
        }
        
        return services.AddSingleton<IHostedService, ConsulHostedService>();
    }
}