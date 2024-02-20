namespace Fake.Authorization.Permissions;

public interface IPermissionDefiner
{
    Task<List<PermissionDto>> DefineAsync();
}