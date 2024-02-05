using System.Collections.Immutable;
using Microsoft.Extensions.Localization;

namespace Fake.Authorization.Permissions;

public class PermissionDto
{
    /// <summary>
    /// 权限名称
    /// </summary>
    public string Name { get; }

    public PermissionDto? Parent { get; set; }

    public IReadOnlyList<PermissionDto> Children => _children.ToImmutableList();
    private readonly List<PermissionDto> _children;

    public LocalizedString DisplayName
    {
        get => _displayName;
        set => _displayName = ThrowHelper.ThrowIfNull(value);
    }

    private LocalizedString _displayName = default!;

    public bool IsEnabled { get; set; }

    public PermissionDto(string name, LocalizedString? displayName = null, bool isEnabled = true)
    {
        Name = name;
        DisplayName = displayName ?? new LocalizedString(name, name);
        IsEnabled = isEnabled;
        _children = new List<PermissionDto>();
    }

    public virtual void AddChild(string name, LocalizedString? displayName = null, bool isEnabled = true)
    {
        var child = new PermissionDto($"{this.Name}.{name}", displayName, isEnabled)
        {
            Parent = this
        };
        _children.Add(child);
    }
}