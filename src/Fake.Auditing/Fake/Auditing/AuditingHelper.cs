using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using Fake.DynamicProxy;
using Fake.Identity.Users;
using Fake.Timing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fake.Auditing;

public class AuditingHelper : IAuditingHelper
{
    private readonly IFakeClock _fakeClock;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<AuditingHelper> _logger;
    private readonly FakeAuditingOptions _options;

    public AuditingHelper(IOptions<FakeAuditingOptions> options, IFakeClock fakeClock, ICurrentUser currentUser,
        ILogger<AuditingHelper> logger)
    {
        _fakeClock = fakeClock;
        _currentUser = currentUser;
        _logger = logger;
        _options = options.Value;
    }

    public virtual bool IsAuditMethod(MethodInfo methodInfo)
    {
        if (!_options.IsEnabledLog) return false;
        if (methodInfo == null) return false;

        if (!methodInfo.IsPublic) return false;

        if (methodInfo.IsDefined(typeof(AuditedAttribute), true)) return true;
        if (methodInfo.IsDefined(typeof(DisableAuditingAttribute), true)) return false;

        if (IsAuditType(methodInfo.DeclaringType)) return true;
        return false;
    }

    public bool IsAuditEntity(Type entityType)
    {
        if (IsAuditType(entityType)) return true;

        foreach (var propertyInfo in entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (propertyInfo.IsDefined(typeof(AuditedAttribute)))
            {
                return true;
            }
        }

        return false;
    }

    public virtual AuditLogInfo CreateAuditLogInfo()
    {
        return new AuditLogInfo()
        {
            ApplicationName = _options.ApplicationName,
            UserId = _currentUser.UserId,
            UserName = _currentUser.UserName,
            ExecutionTime = _fakeClock.Now,
        };
    }

    public AuditLogActionInfo CreateAuditLogActionInfo(IFakeMethodInvocation invocation)
    {
        return new AuditLogActionInfo()
        {
            ServiceName = invocation.TargetObject.GetType().Name,
            MethodName = invocation.Method.Name,
            Parameters = SerializeParameter(invocation.ArgumentsDictionary),
            ExecutionTime = _fakeClock.Now
        };
    }


    public static bool IsAuditType(Type type)
    {
        if (!type.IsPublic) return false;

        //TODO：在继承链中，最好先检查顶层类的attributes
        if (type.IsDefined(typeof(AuditedAttribute), true)) return true;
        if (type.IsDefined(typeof(DisableAuditingAttribute), true)) return false;

        if (type.IsAssignableTo(typeof(IAuditingEnabled))) return true;
        return false;
    }

    protected virtual string SerializeParameter(IReadOnlyDictionary<string, object> actionParameters)
    {
        string defaultParameter = "{}";
        try
        {
            if (actionParameters.IsNullOrEmpty()) return defaultParameter;

            return JsonSerializer.Serialize(actionParameters);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return defaultParameter;
        }
    }
}