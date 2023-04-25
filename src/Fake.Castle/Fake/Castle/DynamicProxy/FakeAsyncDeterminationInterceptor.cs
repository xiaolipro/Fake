using Castle.DynamicProxy;
using Fake.DynamicProxy;

namespace Fake.Castle.DynamicProxy;

public class FakeAsyncDeterminationInterceptor<TInterceptor> : AsyncDeterminationInterceptor
    where TInterceptor : IFakeInterceptor
{
    public FakeAsyncDeterminationInterceptor(TInterceptor fakeInterceptor) : base(
        new FakeAsyncInterceptorAdapter<TInterceptor>(fakeInterceptor))
    {
    }
}