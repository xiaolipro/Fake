using Fake.DependencyInjection;
using Microsoft.AspNetCore.Authorization;

namespace Fake.Authorization.Services;

public class UserService : ITransientDependency
{
    [Authorize("user.create")]
    public virtual Task<int> CreateAsync()
    {
        return Task.FromResult(42);
    }

    public virtual Task<int> NormalAsync()
    {
        return Task.FromResult(42);
    }
}