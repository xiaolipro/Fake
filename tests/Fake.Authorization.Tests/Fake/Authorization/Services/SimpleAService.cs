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