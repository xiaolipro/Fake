using System;
using Fake.Consul.LoadBalancing;
using Fake.LoadBalancing;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.Consul;

public class FakeConsulModule: FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        
        context.Services.Configure<FakeConsulClientOptions>(options =>
        {
            options.Host = "127.0.0.1";
            options.Port = 8080;
            options.GrpcPort = int.TryParse(configuration["GrpcPort"], out var grpcPort) ? grpcPort : 8081;
            options.GrpcHealthCheckPath = "/grpc.health.v1.Health/Check";
            options.ServiceName = "FakeService";
            options.HealthCheckPath = "/health";
            options.Interval = 3;
            options.Timeout = 5;
            options.DeregisterTime = 10;
            options.Weight = 1;
        });

        context.Services.Configure<FakeConsulServiceOptions>(options =>
        {
            options.Address = new Uri("http://localhost:8500"); // 请根据自己的服务地址进行修改
            options.Datacenter = "dc1";
            options.ConfigFileName = "appsettings.Development.json"; // 请根据自己的配置文件名称进行修改
        });

        context.Services.AddSingleton<IServiceBalancer, ConsulPollingServiceBalancer>();
    }
}