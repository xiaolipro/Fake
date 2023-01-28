using Castle.DynamicProxy;
using Fake.Proxy;
using JetBrains.Annotations;

namespace Fake.Castle.DynamicProxy;

public class FakeAsyncDeterminationInterceptor<T> : AsyncDeterminationInterceptor where T : IFakeInterceptor
{
    public FakeAsyncDeterminationInterceptor([NotNull] IAsyncInterceptor asyncInterceptor) : base(asyncInterceptor)
    {
        
    }
}