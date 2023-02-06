using System;
using System.Reflection;
using Fake.DependencyInjection;
using Fake.Timing;
using Microsoft.Extensions.Options;

namespace Fake.Auditing;

public class AuditingHelper : IAuditingHelper
{
    private readonly IClock _clock;
    private readonly FakeAuditingOptions _options;

    public AuditingHelper(IOptions<FakeAuditingOptions> options, IClock clock)
    {
        _clock = clock;
        _options = options.Value;
    }

    public virtual bool IsAuditMethod(MethodInfo methodInfo)
    {
        if (!_options.IsEnabled) return false;
        if (methodInfo == null) return false;

        if (!methodInfo.IsPublic) return false;

        if (methodInfo.IsDefined(typeof(AuditedAttribute), true)) return true;
        if (methodInfo.IsDefined(typeof(DisableAuditingAttribute), true)) return false;

        if (IsAuditType(methodInfo.DeclaringType)) return true;
        return false;
    }

    public virtual AuditLogInfo CreateAuditLogInfo()
    {
        return new AuditLogInfo()
        {
            ApplicationName = _options.ApplicationName,
            ExecutionTime = _clock.Now,
        };
    }
    
    
    public static bool IsAuditType(Type type)
    {
        //TODO：在继承链中，最好先检查顶层类的attributes
        if (type.IsDefined(typeof(AuditedAttribute), true)) return true;
        if (type.IsDefined(typeof(DisableAuditingAttribute), true)) return false;

        if (type.IsAssignableTo(typeof(IAuditingEnabled))) return true;
        return false;
    }
}