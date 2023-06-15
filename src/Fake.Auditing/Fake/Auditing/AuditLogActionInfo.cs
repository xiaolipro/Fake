using System;

namespace Fake.Auditing;

public class AuditLogActionInfo
{
    public string ServiceName { get; set; }

    public string MethodName { get; set; }

    public string Parameters { get; set; }
    
    public DateTime ExecutionTime { get; set; }
    
    public int ExecutionDuration { get; set; }
}