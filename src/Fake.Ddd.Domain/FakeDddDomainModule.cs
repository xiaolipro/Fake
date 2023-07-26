using Fake.Auditing;
using Fake.Domain.Entities.Auditing;
using Fake.EventBus;
using Fake.IDGenerators;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.Ddd.Domain;

[DependsOn(typeof(FakeAuditingModule))]
[DependsOn(typeof(FakeEventBusModule))]
[DependsOn(typeof(FakeUnitOfWorkModule))]
public class FakeDddDomainModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IAuditPropertySetter, DefaultAuditPropertySetter>();
    }
}