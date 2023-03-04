using Fake.DependencyInjection;
using Fake.Logging;
using Fake.Modularity;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.DynamicProxy;

public abstract class AbstractInterceptionTests<TStartupModule> : FakeModuleTestBase<TStartupModule>
    where TStartupModule : FakeModule
{
    protected override void BeforeAddFakeApplication(IServiceCollection services)
    {
        services.OnRegistered(context =>
        {
            if (context.ImplementationType == typeof(SimpleInterceptionTargetClass))
            {
                context.Interceptors.Add<SimpleAsyncInterceptor>();
            }
        });
    }


    [Fact]
    public async Task 拦截异步方法()
    {
        // Arrange
        var target = ServiceProvider.GetService<SimpleInterceptionTargetClass>();
        
        // Action
        await target.DoItAsync();
        
        // Assert
        target.Logs.Count.ShouldBe(5); // 3+2
        target.Logs[0].ShouldBe("SimpleAsyncInterceptor_InterceptAsync_BeforeInvocation");
    }
}


public class SimpleAsyncInterceptor:IFakeInterceptor,ITransientDependency
{
    public async Task InterceptAsync(IFakeMethodInvocation invocation)
    {
        await Task.Delay(5);
        (invocation.TargetObject as ICanLog)?.Logs?.Add($"{GetType().Name}_InterceptAsync_BeforeInvocation");
        await invocation.ProcessAsync();
        (invocation.TargetObject as ICanLog)?.Logs?.Add($"{GetType().Name}_InterceptAsync_AfterInvocation");
        await Task.Delay(5);
    }
}

public class SimpleInterceptionTargetClass : ICanLog,ITransientDependency
{
    public List<string> Logs { get; } = new();
    
    public virtual async Task DoItAsync()
    {
        Logs.Add("EnterDoItAsync");
        await Task.Delay(5);
        Logs.Add("MiddleDoItAsync");
        await Task.Delay(5);
        Logs.Add("ExitDoItAsync");
    }
}