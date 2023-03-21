using Fake.Auditing;
using Fake.Domain.Entities.Auditing;
using Fake.Domain.Entities.IdGenerators;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;


[DependsOn(typeof(FakeAuditingModule))]
[DependsOn(typeof(FakeEventBusModule))]
public class FakeDddDomainModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IGuidGenerator, SequentialGuidGenerator>();
        context.Services.AddTransient<IAuditPropertySetter, DefaultAuditPropertySetter>();

        // 请注意数据库适配问题
        context.Services.Configure<SequentialGuidGeneratorOptions>(options =>
        {
            // 默认生成的有序guid是SequentialAsBinaryAtEnd类型的（SQLSERVER友好的）
            options.SequentialGuidType = SequentialGuidType.SequentialAsString;
        });
    }
}