using System.Threading.Tasks;
using Fake.DependencyInjection;
using Fake.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Xunit;

namespace Fake.Auditing;

public class AuditingTests : FakeAuditingTestBase
{
    protected IAuditingStore AuditingStore;
    private IAuditingManager _auditingManager;

    public AuditingTests()
    {
        _auditingManager = GetRequiredService<IAuditingManager>();
    }

    protected override void AfterAddStartupModule(IServiceCollection services)
    {
        AuditingStore = Substitute.For<IAuditingStore>();
        services.Replace(ServiceDescriptor.Singleton(AuditingStore));
        services.AddSingleton(typeof(IAmbientScopeProvider<>), typeof(AmbientScopeProvider<>));
    }

    [Fact]
    public async Task 打了Audited特性的可以审计()
    {
        var myAuditedObject1 = GetRequiredService<MyAuditedObject1>();

        using (var scope = _auditingManager.BeginScope())
        {
            await myAuditedObject1.DoItAsync(new InputObject { Value1 = "forty-two", Value2 = 42 });
            //await scope.SaveAsync();
        }

        await AuditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
    }
}

public interface IMyAuditedObject : ITransientDependency
{
}

[Audited]
public class MyAuditedObject1 : IMyAuditedObject
{
    public virtual Task<ResultObject> DoItAsync(InputObject inputObject)
    {
        return Task.FromResult(new ResultObject
        {
            Value1 = inputObject.Value1 + "-result",
            Value2 = inputObject.Value2 + 1
        });
    }
}

public class ResultObject
{
    public string Value1 { get; set; }

    public int Value2 { get; set; }
}

public class InputObject
{
    public string Value1 { get; set; }

    public int Value2 { get; set; }
}