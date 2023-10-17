using System;
using System.Reflection;
using Fake.DynamicProxy;

namespace Fake.Auditing;

public interface IAuditingHelper
{
    bool IsAuditMethod(MethodInfo methodInfo);
    bool IsAuditEntity(Type entityType);
    AuditLogInfo CreateAuditLogInfo();
    AuditLogActionInfo CreateAuditLogActionInfo(IFakeMethodInvocation invocation);
}