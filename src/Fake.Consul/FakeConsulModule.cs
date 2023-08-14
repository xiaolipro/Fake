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
        
        context.Services.Configure<FakeConsulRegisterOptions>(configuration.GetSection("ConsulRegister"));

        context.Services.AddConsul(configuration.GetSection("Consul"));
        context.Services.AddSingleton<IServiceBalancer, ConsulPollingServiceBalancer>();
    }
}