using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Fake.DynamicProxy;
using Microsoft.AspNetCore.Authorization;

namespace Fake.Authorization;

public class AuthorizationInterceptor : IFakeInterceptor
{
    public virtual async Task InterceptAsync(IFakeMethodInvocation invocation)
    {
        if (AllowAnonymous(invocation.Method)) return;
    }

    protected virtual bool AllowAnonymous(MethodInfo method)
    {
        return method.GetCustomAttributes(true).OfType<IAllowAnonymous>().Any();
    }
}