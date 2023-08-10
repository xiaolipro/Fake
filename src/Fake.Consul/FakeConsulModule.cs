using System;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.Consul;

public class FakeConsulModule: FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<FakeConsulClientOptions>(options =>
        {
            options.Address = "http://localhost:8500"; // 请根据自己的服务地址进行修改
            options.ServiceName = "FakeService"; // 请根据自己的服务名称进行修改
            options.HealthCheckPath = "/health"; // 请根据自己的服务健康检查路径进行修改
            options.Interval = 3;
            options.Timeout = 5;
            options.DeregisterTime = 10;
            options.Weight = 1;
        });

        context.Services.Configure<FakeConsulServiceOptions>(options =>
        {
            options.Address = new Uri("http://localhost:8500"); // 请根据自己的服务地址进行修改
            options.Datacenter = "dc1";
            options.ConfigFileName = "appsettings.json"; // 请根据自己的配置文件名称进行修改
        });
    }
}