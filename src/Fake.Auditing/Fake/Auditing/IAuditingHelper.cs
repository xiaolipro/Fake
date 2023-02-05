using System.Reflection;

namespace Fake.Auditing;

public interface IAuditingHelper
{
    bool ShouldAuditMethod(MethodInfo methodInfo);
    AuditLogInfo CreateAuditLogInfo();
}