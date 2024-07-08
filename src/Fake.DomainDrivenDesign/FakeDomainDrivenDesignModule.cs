using Fake.Auditing;
using Fake.Domain.Entities.Auditing;
using Fake.EventBus;
using Fake.Localization;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.DomainDrivenDesign;

[DependsOn(typeof(FakeAuditingModule))]
[DependsOn(typeof(FakeEventBusModule))]
[DependsOn(typeof(FakeUnitOfWorkModule))]
[DependsOn(typeof(FakeObjectMappingModule))]
[DependsOn(typeof(FakeLocalizationModule))]
public class FakeDomainDrivenDesignModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IAuditPropertySetter, DefaultAuditPropertySetter>();
    }
}