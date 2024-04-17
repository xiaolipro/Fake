using System.Reflection;
using Fake.DependencyInjection;
using Fake.DynamicProxy;

namespace Fake.Core.Tests.DynamicProxy;

public class SimpleResultCacheInterceptor : IFakeInterceptor, ITransientDependency
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