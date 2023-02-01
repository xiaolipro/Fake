using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Fake.Castle.DynamicProxy;

public class FakeMethodInvocationAdapter : FakeMethodInvocationAdapterBase
{
    private readonly IInvocationProceedInfo _proceedInfo;
    private readonly Func<IInvocation, IInvocationProceedInfo, Task> _proceed;

    public FakeMethodInvocationAdapter(IInvocation invocation, IInvocationProceedInfo proceedInfo,
        Func<IInvocation, IInvocationProceedInfo, Task> proceed) : base(invocation)
    {
        _proceedInfo = proceedInfo;
        _proceed = proceed;
    }

    public override async Task ProcessAsync()
    {
        await _proceed(Invocation, _proceedInfo);
    }
}