using System;
using System.Collections.Generic;

namespace Fake.Auditing;

public class EntityChangeInfo
{
    public DateTime ChangeTime { get; set; }

    public EntityChangeType ChangeType { get; set; }

    public string EntityId { get; set; }

    public string? EntityTypeFullName { get; set; }

    public List<EntityPropertyChangeInfo> PropertyChanges { get; set; }

    public virtual object EntityEntry { get; set; }
}

public class EntityPropertyChangeInfo
{
    public virtual string? NewValue { get; set; }

    public virtual string? OriginalValue { get; set; }

    public virtual string PropertyName { get; set; }

    public virtual string PropertyTypeFullName { get; set; }
}

public enum EntityChangeType : byte
{
    Created = 0,

    Updated = 1,

    Deleted = 2,
}