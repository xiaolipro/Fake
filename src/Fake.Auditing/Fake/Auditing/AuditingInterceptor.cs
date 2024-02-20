using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Fake.DynamicProxy;
using Fake.UnitOfWork;
using Fake.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fake.Auditing;

public class AuditingInterceptor : IFakeInterceptor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AuditingInterceptor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public virtual async Task InterceptAsync(IFakeMethodInvocation invocation)
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

        // tips：创建新的审计scope，只有此scope会在最终save
        var currentUser = serviceScope.ServiceProvider.GetRequiredService<ICurrentUser>();
        var unitOfWorkManager = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        await ProcessWithNewAuditingScopeAsync(invocation, auditingOptions, auditingManager, auditingHelper,
            currentUser, unitOfWorkManager);
    }

    private async Task ProcessWithNewAuditingScopeAsync(IFakeMethodInvocation invocation,
        FakeAuditingOptions auditingOptions, IAuditingManager auditingManager, IAuditingHelper auditingHelper,
        ICurrentUser currentUser, IUnitOfWorkManager unitOfWorkManager)
    {
        var hasError = false;

        using var scope = auditingManager.BeginScope();
        try
        {
            Debug.Assert(auditingManager.Current != null, "auditingManager.Current != null");

            await ProcessWithCurrentAuditingScopeAsync(invocation, auditingOptions, auditingHelper,
                auditingManager.Current!.Log);

            if (auditingManager.Current!.Log.Exceptions.Any()) hasError = true;
        }
        catch (Exception)
        {
            hasError = true;
            throw;
        }
        finally
        {
            if (await ShouldSaveAsync(invocation, auditingOptions, auditingManager.Current!.Log, hasError, currentUser))
            {
                /*
                 * promise：在save审计前，一定先触发uow的SaveChangesAsync
                 */
                if (unitOfWorkManager.Current != null)
                {
                    try
                    {
                        await unitOfWorkManager.Current.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        auditingManager.Current.Log.Exceptions.TryAdd(ex);
                    }
                }

                await scope.SaveAsync();
            }
        }
    }

    private async Task<bool> ShouldSaveAsync(IFakeMethodInvocation invocation, FakeAuditingOptions auditingOptions,
        AuditLogInfo log, bool hasError, ICurrentUser currentUser)
    {
        foreach (var selector in auditingOptions.LogSelectors)
        {
            if (await selector(log))
            {
                return true;
            }
        }

        if (auditingOptions.IsEnabledExceptionLog && hasError)
        {
            return true;
        }

        if (!auditingOptions.AllowAnonymous && !currentUser.IsAuthenticated) return false;


        if (!auditingOptions.IsEnabledGetRequestLog &&
            invocation.Method.Name.StartsWith(HttpMethod.Get.ToString(), StringComparison.OrdinalIgnoreCase)
           )
            return false;

        return true;
    }

    private async Task ProcessWithCurrentAuditingScopeAsync(IFakeMethodInvocation invocation,
        FakeAuditingOptions auditingOptions,
        IAuditingHelper auditingHelper, AuditLogInfo auditLogInfo)
    {
        AuditLogActionInfo? auditLogActionInfo = null;
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