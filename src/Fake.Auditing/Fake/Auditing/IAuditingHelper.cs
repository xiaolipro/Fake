using System.Reflection;

namespace Fake.Auditing;

public interface IAuditingHelper
{
    bool IsAuditMethod(MethodInfo methodInfo);
    AuditLogInfo CreateAuditLogInfo();
}