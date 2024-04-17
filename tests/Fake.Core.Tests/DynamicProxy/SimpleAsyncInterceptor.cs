using Fake.DependencyInjection;
using Fake.DynamicProxy;
using Fake.Logging;

namespace Fake.Core.Tests.DynamicProxy;

public class SimpleAsyncInterceptor : IFakeInterceptor, ITransientDependency
{
    public SimpleAsyncInterceptor()
    {
    }

    public async Task InterceptAsync(IFakeMethodInvocation invocation)
    {
        await Task.Delay(5);
        (invocation.TargetObject as ICanLog)?.Logs?.Add($"{GetType().Name}_InterceptAsync_BeforeInvocation");
        await invocation.ProcessAsync();
        (invocation.TargetObject as ICanLog)?.Logs?.Add($"{GetType().Name}_InterceptAsync_AfterInvocation");
        await Task.Delay(5);
    }
}