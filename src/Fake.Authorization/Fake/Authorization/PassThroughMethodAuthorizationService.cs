using System.Threading.Tasks;

namespace Fake.Authorization;

/// <summary>
/// 方法免授权
/// </summary>
public class PassThroughMethodAuthorizationService : IMethodAuthorizationService
{
    public Task CheckAsync(MethodAuthorizationContext context)
    {
        return Task.CompletedTask;
    }
}