using System;
using Consul;
using Fake;
using Fake.Consul;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConsulServiceCollectionExtensions
{
    /// <summary>
    /// 添加Consul
    /// </summary>
    /// <param name="services"></param>
    /// <param name="action"></param>
    internal static IServiceCollection AddConsul(this IServiceCollection services,
        Action<FakeConsulClientOptions>? action = null)
    {
        var configuration = services.GetConfiguration();
        services.Configure<FakeConsulClientOptions>(configuration.GetSection("Consul:Client"));

        var consulClientOptions = configuration.Get<FakeConsulClientOptions>() ?? new FakeConsulClientOptions();
        ThrowHelper.ThrowIfNull(consulClientOptions, nameof(consulClientOptions), "Consul配置为空");
        action?.Invoke(consulClientOptions);

        services.Configure<FakeConsulRegisterOptions>(configuration.GetSection("Consul:Register"));

        services.AddSingleton<IConsulClient, ConsulClient>(_ => new ConsulClient(consulConfig =>
        {
            consulConfig.Address = consulClientOptions.Address;
            consulConfig.Datacenter = consulClientOptions.Datacenter;
            consulConfig.Token = consulClientOptions.Token;
        }));

        return services;
    }
}