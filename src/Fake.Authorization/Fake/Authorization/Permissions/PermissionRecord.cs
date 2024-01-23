namespace Fake.Authorization.Permissions;

public record PermissionRecord
{
    public PermissionRecord(string name)
    {
        Name = name;
    }

    /// <summary>
    /// 权限名称
    /// </summary>
    public string Name { get; }
}