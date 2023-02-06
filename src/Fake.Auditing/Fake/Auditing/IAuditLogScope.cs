using JetBrains.Annotations;

namespace Fake.Auditing;

public interface IAuditLogScope
{
    [NotNull]
    AuditLogInfo Log { get; }
}