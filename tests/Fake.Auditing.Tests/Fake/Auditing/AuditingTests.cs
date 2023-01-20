using System.Threading.Tasks;
using Fake.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Xunit;

namespace Fake.Auditing;

public class AuditingTests:FakeAuditingTestBase
{
    protected IAuditingStore AuditingStore;
    
    public AuditingTests()
    {
    }

    protected override void AfterAddStartupModule(ServiceCollection services)
    {
        AuditingStore = Substitute.For<IAuditingStore>();
        services.Replace(ServiceDescriptor.Singleton(AuditingStore));
    }

    [Fact]
    public async Task Should_Write_AuditLog_For_Classes_That_Implement_IAuditingEnabled_Without_An_Explicit_Scope()
    {
        var myAuditedObject1 = GetRequiredService<MyAuditedObject1>();

        await myAuditedObject1.DoItAsync(new InputObject { Value1 = "forty-two", Value2 = 42 });

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