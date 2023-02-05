using JetBrains.Annotations;

namespace Fake.Auditing;

public interface IAuditingManager
{
    [CanBeNull] AuditLogScope Current { get; }
    
    IAuditLogSaveHandle BeginScope();
}