using Fake.Auditing;
using Fake.DomainDrivenDesign.Entities.Auditing;
using Fake.EventBus;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.DomainDrivenDesign;

[DependsOn(typeof(FakeAuditingModule))]
[DependsOn(typeof(FakeEventBusModule))]
[DependsOn(typeof(FakeUnitOfWorkModule))]
public class FakeDomainDrivenDesignModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IAuditPropertySetter, DefaultAuditPropertySetter>();
    }
}