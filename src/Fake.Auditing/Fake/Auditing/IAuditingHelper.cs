using System.Reflection;
using Fake.DynamicProxy;

namespace Fake.Auditing;

public interface IAuditingHelper
{
    bool IsAuditMethod(MethodInfo methodInfo);
    AuditLogInfo CreateAuditLogInfo();
    AuditLogActionInfo CreateAuditLogActionInfo(IFakeMethodInvocation invocation);
}