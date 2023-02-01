using System.Reflection;

namespace Fake.Auditing;

public interface IAuditingHelper
{
    bool ShouldSaveAudit(MethodInfo methodInfo);
    AuditLogInfo CreateAuditLogInfo();
}