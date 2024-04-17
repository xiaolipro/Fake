using Fake.Authorization;
using Fake.DependencyInjection;

namespace Fake.AspNetCore.Tests.ExceptionHandling;

public class SimpleService : ITransientDependency
{
    public virtual void AuthorizationException()
    {
        throw new FakeAuthorizationException("授权失败");
    }
}