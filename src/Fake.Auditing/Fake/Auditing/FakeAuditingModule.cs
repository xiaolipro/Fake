using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Auditing;

[DependsOn(typeof(FakeAuditingContractsModule))]
public class FakeAuditingModule:FakeModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(AuditingInterceptorRegistrar.RegisterIfNeeded);
    }
}

public class AuditingInterceptor
{
    
}