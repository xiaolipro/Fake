using Fake.Modularity;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Core.Tests.DynamicProxy;

public abstract class DisableClassInterceptionTestBase<TStartupModule> : ApplicationTest<TStartupModule>
    where TStartupModule : FakeModule
{
    protected override void BeforeAddFakeApplication(IServiceCollection services)
    {
        base.BeforeAddFakeApplication(services);
        services.DisableClassInterceptors();
        services.AddTransient<ISimpleInterceptionTarget, SimpleInterceptionTargetClass>();
    }

    [Fact]
    public async Task 禁用类拦截器()
    {
        // Arrange
        var target = ServiceProvider.GetService<SimpleInterceptionTargetClass>();

        // Action
        await target.DoItAsync();

        // Assert
        target.Logs.Count.ShouldBe(3);
        target.Logs[0].ShouldBe("EnterDoItAsync");
    }


    [Fact]
    public async Task 禁用类拦截器不影响接口拦截()
    {
        // Arrange
        var target = ServiceProvider.GetService<SimpleInterceptionTargetClass>();
        var interfaceTarget = ServiceProvider.GetService<ISimpleInterceptionTarget>();

        // Action
        await target.DoItAsync();
        await interfaceTarget.DoItAsync();

        // Assert
        target.Logs.Count.ShouldBe(3);
        target.Logs[0].ShouldBe("EnterDoItAsync");

        // 接口会拦截所有成员（包括属性），而类拦截器只拦截虚方法
        interfaceTarget.Logs.Count.ShouldBe(7); // +2
        interfaceTarget.Logs[0].ShouldBe("SimpleAsyncInterceptor_InterceptAsync_BeforeInvocation"); // +2
        interfaceTarget.Logs.Count.ShouldBe(11); // +2
    }

    [Fact]
    public void 禁用类拦截器不影响接口拦截2()
    {
        // Arrange
        var interfaceTarget = ServiceProvider.GetService<ISimpleInterceptionTarget>();
        // Assert
        interfaceTarget.Logs.Count.ShouldBe(2);
        interfaceTarget.Logs.Count.ShouldBe(4);
    }
}