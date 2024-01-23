﻿using Fake.Modularity;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.DynamicProxy;

public abstract class InterceptionTestBase<TStartupModule> : ApplicationTest<TStartupModule>
    where TStartupModule : FakeModule
{
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
        var target = ServiceProvider.GetRequiredService<SimpleResultCacheInterceptionTargetClass>();

        // Act & Assert
        (await target.GetValueAsync(42)).ShouldBe(42); //First run, not cached yet
        (await target.GetValueAsync(43)).ShouldBe(42); //First run, cached previous value
        (await target.GetValueAsync(44)).ShouldBe(42); //First run, cached previous value


        // TODO：Interceptors failed to set a return value, or swallowed the exception thrown by the target
        // target.GetValue(1).ShouldBe(1); //First run, not cached yet
        // target.GetValue(44).ShouldBe(1); //First run, cached previous value
    }
}