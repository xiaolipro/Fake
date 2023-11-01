using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Fake.Authorization.Services;

[Authorize]
public class SimpleService
{
    public Task<int> ProtectedByClassAsync()
    {
        return Task.FromResult(42);
    }

    [AllowAnonymous]
    public Task<int> AnonymousAsync()
    {
        return Task.FromResult(42);
    }
}