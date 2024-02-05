using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fake.Authorization.Permissions;

public class NullPermissionStore : IPermissionStore
{
    public ILogger<NullPermissionStore> Logger { get; set; } = NullLogger<NullPermissionStore>.Instance;

    public Task<bool> IsGrantedAsync(string permission, string ownerName, string ownerKey)
    {
        return Task.FromResult(false);
    }
}