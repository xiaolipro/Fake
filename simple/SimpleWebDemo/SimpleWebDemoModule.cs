using Fake.AspNetCore;
using Fake.Auditing;
using Fake.Autofac;
using Fake.Modularity;

namespace SimpleWebDemo;

[DependsOn(typeof(FakeAuditingModule), typeof(FakeAutofacModule))]
[DependsOn(typeof(FakeAspNetCoreModule))]
public class SimpleWebDemoModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<FakeAuditingOptions>(options =>
        {
            // options.IsEnabledActionLog = false;
            // options.IsEnabledExceptionLog = false;
        });
    }
}