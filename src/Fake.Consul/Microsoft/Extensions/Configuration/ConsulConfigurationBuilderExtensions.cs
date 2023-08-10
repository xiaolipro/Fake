using System;
using Fake.Consul;
using Winton.Extensions.Configuration.Consul;

namespace Microsoft.Extensions.Configuration;

public static class ConsulConfigurationBuilderExtensions
{
    /// <summary>
    /// 加载Consul上的配置文件
    /// </summary>
    /// <returns></returns>
    public static IConfigurationRoot AddConfigurationFromConsul(this IConfigurationBuilder builder)
    {
        var configuration = builder.Build();
        var consulClientOptions = configuration.GetSection("Consul").Get<FakeConsulServiceOptions>();
        
        // 加载Consul上的配置文件
        builder.AddConsul(consulClientOptions.ConfigFileName, sources =>
        {
            sources.ConsulConfigurationOptions = options =>
            {
                options.Address = consulClientOptions.Address;
                options.Datacenter = consulClientOptions.Datacenter;
            };
            sources.Optional = true;
            sources.ReloadOnChange = true; // hot-update
        });

        return builder.Build();
    }
}