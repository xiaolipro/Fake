using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Fake.Proxy;

namespace Fake.Castle.DynamicProxy;

public class FakeInterceptorAdapter<TInterceptor> : AsyncInterceptorBase where TInterceptor : IFakeInterceptor
{
    private readonly TInterceptor _fakeInterceptor;

    public FakeInterceptorAdapter(TInterceptor fakeInterceptor)
    {
        _fakeInterceptor = fakeInterceptor;
    }

    protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo,
        Func<IInvocation, IInvocationProceedInfo, Task> proceed)
    {
        // 适配器模式
        var adapter = new FakeMethodInvocationAdapter(invocation, proceedInfo, proceed);
        await _fakeInterceptor.InterceptAsync(adapter);
    }

    protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo,
        Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
    {
        var adapter = new FakeMethodInvocationAdapter(invocation, proceedInfo, proceed);
        
        await _fakeInterceptor.InterceptAsync(adapter);

        return (TResult)adapter.ReturnValue;
    }
}