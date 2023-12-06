using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fake.Authorization.Permissions;

public class PermissionRecordStore : IPermissionRecordStore
{
    public Task<PermissionRecord?> GetOrNullAsync(string name)
    {
        throw new System.NotImplementedException();
    }

    public Task<IReadOnlyList<PermissionRecord>> GetPermissionsAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task<IReadOnlyList<PermissionRecord>> GetGroupsAsync()
    {
        throw new System.NotImplementedException();
    }
}