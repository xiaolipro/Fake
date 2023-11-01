using System.Reflection;
using System.Threading.Tasks;

namespace Fake.Authorization;

public class AlwaysAllowMethodAuthorizationService : IMethodAuthorizationService
{
    public Task<bool> IsGrantedAsync(MethodInfo invocationMethod)
    {
        return Task.FromResult<bool>(true);
    }
}