using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using Fake.DynamicProxy;
using Fake.Timing;
using Fake.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fake.Auditing;

public class AuditingHelper(
    IOptions<FakeAuditingOptions> options,
    IFakeClock fakeClock,
    ICurrentUser currentUser,
    ILogger<AuditingHelper> logger,
    IServiceScopeFactory serviceScopeFactory)
    : IAuditingHelper
{
    protected readonly IFakeClock FakeClock = fakeClock;
    protected readonly ICurrentUser CurrentUser = currentUser;
    protected readonly ILogger<AuditingHelper> Logger = logger;
    protected readonly IServiceScopeFactory ServiceScopeFactory = serviceScopeFactory;
    protected readonly FakeAuditingOptions FakeAuditingOptions = options.Value;

    public virtual bool IsAuditMethod(MethodInfo methodInfo)
    {
        if (!FakeAuditingOptions.IsEnabledLog) return false;

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
        var auditLog = new AuditLogInfo()
        {
            ApplicationName = FakeAuditingOptions.ApplicationName,
            UserId = CurrentUser.Id,
            UserName = CurrentUser.UserName,
            ExecutionTime = FakeClock.Now,
        };

        ExecutePreContributors(auditLog);

        return auditLog;
    }

    public AuditLogActionInfo CreateAuditLogActionInfo(IFakeMethodInvocation invocation)
    {
        return new AuditLogActionInfo()
        {
            ServiceName = invocation.TargetObject.GetType().Name,
            MethodName = invocation.Method.Name,
            Parameters = SerializeParameter(invocation.ArgumentsDictionary),
            ExecutionTime = FakeClock.Now
        };
    }


    public static bool IsAuditType(Type? type)
    {
        if (type == null) return false;
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
            Logger.LogWarning(ex.Message);
            return defaultParameter;
        }
    }

    protected virtual void ExecutePreContributors(AuditLogInfo auditLogInfo)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var context = new AuditLogContributionContext(scope.ServiceProvider, auditLogInfo);

        foreach (var contributor in FakeAuditingOptions.Contributors)
        {
            try
            {
                contributor.PreContribute(context);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, LogLevel.Warning);
            }
        }
    }
}