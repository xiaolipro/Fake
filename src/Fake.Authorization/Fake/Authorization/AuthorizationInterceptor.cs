using System.Threading.Tasks;
using Fake.DynamicProxy;

namespace Fake.Authorization;

public class AuthorizationInterceptor(IMethodAuthorizationService methodAuthorizationService) : IFakeInterceptor
{
    public virtual async Task InterceptAsync(IFakeMethodInvocation invocation)
    {
        await methodAuthorizationService.CheckAsync(new MethodAuthorizationContext(invocation.Method));
        await invocation.ProcessAsync();
    }
}