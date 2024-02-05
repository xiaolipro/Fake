using Fake.DependencyInjection;

namespace Fake.Authorization.Permissions;

public class FakePermissionStore : IPermissionStore, ITransientDependency
{
    public Task<bool> IsGrantedAsync(string permission, string ownerName, string ownerKey)
    {
        return Task.FromResult(permission.StartsWith("user"));
    }
}