using System;

namespace Fake.Auditing;

[Serializable]
public class AuditLogActionInfo
{
    public string ServiceName { get; set; } = default!;

    public string MethodName { get; set; } = default!;

    public string Parameters { get; set; } = default!;

    public DateTime ExecutionTime { get; set; }

    public int ExecutionDuration { get; set; }
}