using System;

namespace Fake.Auditing;

[Serializable]
public class EntityPropertyChangeInfo
{
    public virtual string? NewValue { get; set; }

    public virtual string? OriginalValue { get; set; }

    public virtual string PropertyName { get; set; } = default!;

    public virtual string PropertyTypeFullName { get; set; } = default!;
}