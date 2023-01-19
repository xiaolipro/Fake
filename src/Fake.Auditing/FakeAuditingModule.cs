using Fake.Auditing;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

[DependsOn(typeof(FakeAuditingContractsModule))]
public class FakeAuditingModule:FakeModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(AuditingInterceptorRegistrar.RegisterIfNeeded);
    }
}