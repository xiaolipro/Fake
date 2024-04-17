using Fake.Castle.DynamicProxy;
using Fake.DependencyInjection;

namespace Fake.Core.Tests.DynamicProxy;

[FakeIntercept(typeof(SimpleResultCacheInterceptor))]
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
        return 42;
    }

    public void Ff(int v)
    {
    }
}