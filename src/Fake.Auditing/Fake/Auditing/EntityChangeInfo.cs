using System;

namespace Fake.Auditing;

public class EntityChangeInfo
{
    public DateTime ChangeTime { get; set; }

    public EntityChangeType ChangeType { get; set; }
}

public enum EntityChangeType : byte
{
    Created = 0,

    Updated = 1,

    Deleted = 2
}