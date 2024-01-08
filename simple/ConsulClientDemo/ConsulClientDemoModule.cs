using Fake.AspNetCore;
using Fake.Consul;
using Fake.Modularity;

[DependsOn(typeof(FakeAspNetCoreModule))]
[DependsOn(typeof(FakeConsulModule))]
public class ConsulClientDemoModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}