using System.Collections.Immutable;
using Microsoft.Extensions.Localization;

namespace Fake.Authorization.Permissions;

public class Permission
{
    /// <summary>
    /// 权限名称
    /// </summary>
    public string Name { get; }

    public Permission? Parent { get; set; }

    public IReadOnlyList<Permission> Children => _children.ToImmutableList();
    private readonly List<Permission> _children;

    public LocalizedString DisplayName
    {
        get => _displayName;
        set => _displayName = ThrowHelper.ThrowIfNull(value);
    }

    private LocalizedString _displayName = default!;

    public bool IsEnabled { get; set; }

    private Permission(string name, LocalizedString? displayName = null, bool isEnabled = true)
    {
        Name = name;
        DisplayName = displayName ?? new LocalizedString(name, name);
        IsEnabled = isEnabled;
        _children = new List<Permission>();
    }

    public virtual void AddChild(Permission child)
    {
        child.Parent = this;
        _children.Add(child);
    }
}