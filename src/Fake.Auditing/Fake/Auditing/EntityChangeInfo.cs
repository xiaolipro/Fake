using System;
using System.Collections.Generic;

namespace Fake.Auditing;

[Serializable]
public class EntityChangeInfo
{
    public DateTime ChangeTime { get; set; }

    public EntityChangeType ChangeType { get; set; }

    public string? EntityId { get; set; }

    public string? EntityTypeFullName { get; set; }

    public List<EntityPropertyChangeInfo> PropertyChanges { get; set; } = default!;

    public virtual object EntityEntry { get; set; } = default!;
}