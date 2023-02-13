using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Fake.DependencyInjection;
using Fake.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fake.Auditing;

public class AuditingInterceptor : IFakeInterceptor, ITransientDependency
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AuditingInterceptor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InterceptAsync(IFakeMethodInvocation invocation)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();

        var auditingHelper = serviceScope.ServiceProvider.GetRequiredService<IAuditingHelper>();

        if (!auditingHelper.IsAuditMethod(invocation.Method))
        {
            await invocation.ProcessAsync();
            return;
        }

        var auditingManager = serviceScope.ServiceProvider.GetRequiredService<IAuditingManager>();
        var auditingOptions = serviceScope.ServiceProvider.GetRequiredService<IOptions<FakeAuditingOptions>>().Value;

        if (auditingManager.Current != null)
        {
            await ProcessWithCurrentAuditingScopeAsync(invocation, auditingOptions, auditingHelper,
                auditingManager.Current.Log);
            return;
        }

        // 使用新的auditing-scope处理
        await ProcessWithNewAuditingScopeAsync(invocation, auditingOptions, auditingManager, auditingHelper);
    }

    private async Task ProcessWithNewAuditingScopeAsync(IFakeMethodInvocation invocation,
        FakeAuditingOptions auditingOptions, IAuditingManager auditingManager, IAuditingHelper auditingHelper)
    {
        var hasError = false;

        using var scope = auditingManager.BeginScope();
        try
        {
            Debug.Assert(auditingManager.Current != null, "auditingManager.Current != null");
            await ProcessWithCurrentAuditingScopeAsync(invocation, auditingOptions, auditingHelper,
                auditingManager.Current.Log);

            if (auditingManager.Current.Log.Exceptions.Any()) hasError = true;
        }
        catch (Exception)
        {
            hasError = true;
            throw;
        }
        finally
        {
            if (await ShouldSaveAsync(invocation, auditingOptions, auditingManager.Current!.Log, hasError))
            {
                await scope.SaveAsync();
            }
        }
    }

    private async Task<bool> ShouldSaveAsync(IFakeMethodInvocation invocation, FakeAuditingOptions auditingOptions,
        AuditLogInfo log, bool hasError)
    {
        foreach (var selector in auditingOptions.LogSelectors)
        {
            return await selector(log);
        }

        if (auditingOptions.IsEnabledExceptionLog && hasError)
        {
            return true;
        }

        if (!auditingOptions.IsEnabledGetRequestLog &&
            invocation.Method.Name.StartsWith("Get", StringComparison.OrdinalIgnoreCase)
           )
            return false;

        return true;
    }

    private async Task ProcessWithCurrentAuditingScopeAsync(IFakeMethodInvocation invocation,
        FakeAuditingOptions auditingOptions,
        IAuditingHelper auditingHelper, AuditLogInfo auditLogInfo)
    {
        AuditLogActionInfo auditLogActionInfo = null;
        if (auditingOptions.IsEnabledActionLog)
        {
            auditLogActionInfo = auditingHelper.CreateAuditLogActionInfo(invocation);
        }

        var stopwatch = Stopwatch.StartNew();

        try
        {
            await invocation.ProcessAsync();
        }
        catch (Exception e)
        {
            if (auditingOptions.IsEnabledExceptionLog)
            {
                auditLogInfo.Exceptions.Add(e);
            }

            throw;
        }
        finally
        {
            stopwatch.Stop();

            if (auditLogActionInfo != null)
            {
                auditLogActionInfo.ExecutionDuration = (int)stopwatch.Elapsed.TotalMilliseconds;
                auditLogInfo.Actions.Add(auditLogActionInfo);
            }
        }
    }
}