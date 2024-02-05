using Fake.DependencyInjection;
using Microsoft.AspNetCore.Authorization;

namespace Fake.Authorization.Services;

public class SystemService : ITransientDependency
{
    [Authorize]
    public virtual Task DoSomethingAsync()
    {
        return Task.CompletedTask;
    }

    [Authorize(Roles = "admin")]
    public virtual Task ProtectedByRoleAsync()
    {
        return Task.CompletedTask;
    }

    [Authorize(AuthenticationSchemes = "Bearer,qq")]
    public virtual Task<int> ProtectedByScheme()
    {
        return Task.FromResult(42);
    }
}