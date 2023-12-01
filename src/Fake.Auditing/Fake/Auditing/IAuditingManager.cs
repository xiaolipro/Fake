using JetBrains.Annotations;

namespace Fake.Auditing;

public interface IAuditingManager
{
    [CanBeNull] IAuditLogScope? Current { get; }
    
    IAuditLogSaveHandle BeginScope();
}