using Fake.Consul;
using Fake.Modularity;

namespace ConsulClientDemo;

[DependsOn(typeof(FakeAspNetCoreModule))]
[DependsOn(typeof(FakeConsulModule))]
public class ConsulClientDemoModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}