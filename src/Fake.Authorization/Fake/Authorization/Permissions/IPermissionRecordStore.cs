using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fake.Authorization.Permissions;

public interface IPermissionRecordStore
{
    Task<PermissionRecord?> GetOrNullAsync(string name);

    Task<IReadOnlyList<PermissionRecord>> GetPermissionsAsync();

    Task<IReadOnlyList<PermissionRecord>> GetGroupsAsync();
}