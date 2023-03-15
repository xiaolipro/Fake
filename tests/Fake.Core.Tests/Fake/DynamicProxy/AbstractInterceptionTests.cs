using System.Reflection;
using Fake.DependencyInjection;
using Fake.Logging;
using Fake.Modularity;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.DynamicProxy;

public abstract class AbstractInterceptionTests<TStartupModule> : FakeIntegrationTest<TStartupModule>
    where TStartupModule : FakeModule
{
    protected override void BeforeAddFakeApplication(IServiceCollection services)
    {
        services.OnRegistered(context =>
        {
            if (context.ImplementationType == typeof(SimpleInterceptionTargetClass))
            {
                context.Interceptors.TryAdd<SimpleAsyncInterceptor>();
            }

            if (context.ImplementationType == typeof(SimpleResultCacheInterceptionTargetClass))
            {
                context.Interceptors.TryAdd<SimpleResultCacheInterceptor>();
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


    [Fact]
    public async Task 拦截器缓存方法结果()
    {
        // Arrange
        var target = ServiceProvider.GetService<SimpleResultCacheInterceptionTargetClass>();

        // Act & Assert
        (await target.GetValueAsync(42)).ShouldBe(42); //First run, not cached yet
        (await target.GetValueAsync(43)).ShouldBe(42); //First run, cached previous value
        (await target.GetValueAsync(44)).ShouldBe(42); //First run, cached previous value
        
        
        // TODO：Interceptors failed to set a return value, or swallowed the exception thrown by the target
        // target.GetValue(1).ShouldBe(1); //First run, not cached yet
        // target.GetValue(44).ShouldBe(1); //First run, cached previous value
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

public class SimpleResultCacheInterceptionTargetClass : ITransientDependency
{
    public virtual int GetValue(int v)
    {
        Thread.Sleep(5);
        return v;
    }

    public virtual async Task<int> GetValueAsync(int v)
    {
        await Task.Delay(5);
        return v;
    }

    public void FF(int v)
    {
        
    }
}

public class SimpleResultCacheInterceptor : IFakeInterceptor,ITransientDependency
{
    private readonly ConcurrentDictionary<MethodInfo, object> _cache;

    public SimpleResultCacheInterceptor()
    {
        _cache = new ConcurrentDictionary<MethodInfo, object>();
    }
    public async Task InterceptAsync(IFakeMethodInvocation invocation)
    {
        if (_cache.ContainsKey(invocation.Method))
        {
            invocation.ReturnValue = _cache[invocation.Method];
            return;
        }

        await invocation.ProcessAsync();
        _cache[invocation.Method] = invocation.ReturnValue;
    }
}