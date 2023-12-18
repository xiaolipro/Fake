using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Fake.Castle.DynamicProxy;

public class FakeMethodInvocationAdapterWithReturnValue<TResult> : FakeMethodInvocationAdapterBase
{
    private readonly IInvocationProceedInfo _proceedInfo;
    private readonly Func<IInvocation, IInvocationProceedInfo, Task<TResult>> _proceed;

    public FakeMethodInvocationAdapterWithReturnValue(IInvocation invocation,
        IInvocationProceedInfo proceedInfo,
        Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed) : base(invocation)
    {
        _proceedInfo = proceedInfo;
        _proceed = proceed;
    }

    public override async Task ProcessAsync()
    {
        ReturnValue = await _proceed(Invocation, _proceedInfo);
    }
}