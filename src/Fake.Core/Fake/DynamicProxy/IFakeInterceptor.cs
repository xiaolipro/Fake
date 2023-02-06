namespace Fake.DynamicProxy;

public interface IFakeInterceptor
{
    Task InterceptAsync(IFakeMethodInvocation invocation);
}