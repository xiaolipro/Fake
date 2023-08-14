using Fake.AspNetCore;
using Fake.Consul;
using Fake.Modularity;

namespace ConsulServerDemo;

[DependsOn(typeof(FakeConsulModule))]
[DependsOn(typeof(FakeAspNetCoreModule))]
public class ConsulServerDemoModule:FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configure = context.Services.GetConfiguration();
        context.Services.Configure<Student>(configure.GetSection("Student"));
    }
}