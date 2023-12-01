namespace Fake.Auditing;

public interface IAuditLogScope
{
    AuditLogInfo Log { get; }
}