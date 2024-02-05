using Fake.DependencyInjection;
using Microsoft.AspNetCore.Authorization;

namespace Fake.Authorization.Services;

[Authorize]
public class SimpleBService : ITransientDependency
{
    public virtual Task<int> ProtectedByClassAsync()
    {
        return Task.FromResult(42);
    }

    [AllowAnonymous]
    public virtual Task<int> AnonymousAsync()
    {
        return Task.FromResult(42);
    }
}