using System.Threading.Tasks;
using Fake.DependencyInjection;
using Microsoft.AspNetCore.Authorization;

namespace Fake.Authorization.Services;

public class SimpleAService : ITransientDependency
{
    [Authorize]
    public virtual Task<int> ProtectedAsync()
    {
        return Task.FromResult(42);
    }

    public virtual Task<int> NormalAsync()
    {
        return Task.FromResult(42);
    }
}

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