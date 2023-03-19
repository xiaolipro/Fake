using Fake.Auditing;
using Fake.Domain.Entities.Auditing;
using Fake.Domain.Entities.IdGenerators;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;


[DependsOn(typeof(FakeAuditingModule))]
public class FakeDddDomainModule:FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IGuidGenerator, SequentialGuidGenerator>();
        context.Services.AddTransient<IAuditPropertySetter, DefaultAuditPropertySetter>();
    }
}