using Fake.Consul.LoadBalancing;
using Fake.LoadBalancing;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.Consul;

public class FakeConsulModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddConsul();
        context.Services.AddHostedService<ConsulHostedService>();
        context.Services.AddSingleton<IServiceBalancer, ConsulPollingServiceBalancer>();
    }
}