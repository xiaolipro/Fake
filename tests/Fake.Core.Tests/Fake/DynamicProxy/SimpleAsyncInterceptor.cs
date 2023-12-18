using Fake.DependencyInjection;
using Fake.Logging;

namespace Fake.DynamicProxy;

public class SimpleAsyncInterceptor : IFakeInterceptor, ITransientDependency
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