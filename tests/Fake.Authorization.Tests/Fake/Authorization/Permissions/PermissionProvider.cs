using Fake.DependencyInjection;

namespace Fake.Authorization.Permissions;

public class PermissionProvider : IPermissionProvider, ITransientDependency
{
    public Task<List<Permission>> GetPermissionsAsync()
    {
        var res = new List<Permission>();
        var userPermission = new Permission("user");
        userPermission.AddChild("create");
        userPermission.AddChild("edit");
        res.Add(userPermission);
        return Task.FromResult(res);
    }
}