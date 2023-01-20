using System;

namespace Fake.Auditing;

[Serializable]
public class AuditLogInfo
{
    public string ApplicationName { get; set; }
}