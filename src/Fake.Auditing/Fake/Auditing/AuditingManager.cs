using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Fake.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Fake.Auditing;

public class AuditingManager(
    IAmbientScopeProvider<IAuditLogScope?> ambientScopeProvider,
    IAuditingHelper auditingHelper,
    IAuditingStore auditingStore,
    IServiceProvider serviceProvider)
    : IAuditingManager
{
    private const string AuditingContextKey = "Fake.Auditing.AuditLogScope";

    private readonly ILogger<AuditingManager> _logger = NullLogger<AuditingManager>.Instance;

    public IAuditLogScope? Current => ambientScopeProvider.GetValue(AuditingContextKey);

    public IAuditLogSaveHandle BeginScope()
    {
        var value = new AuditLogScope(auditingHelper.CreateAuditLogInfo());
        var scope = ambientScopeProvider.BeginScope(AuditingContextKey, value);

        Debug.Assert(Current != null, nameof(Current) + " != null");
        return new AuditLogSaveHandle(this, scope, Current!.Log, Stopwatch.StartNew());
    }

    protected virtual async Task SaveAsync(AuditLogSaveHandle saveHandle)
    {
        BeforeSave(saveHandle);
        await auditingStore.SaveAsync(saveHandle.AuditLog);
    }

    protected virtual void BeforeSave(AuditLogSaveHandle saveHandle)
    {
        saveHandle.StopWatch.Stop();
        saveHandle.AuditLog.ExecutionDuration = Convert.ToInt32(saveHandle.StopWatch.Elapsed.TotalMilliseconds);
        ExecutePostContributors(saveHandle.AuditLog);
        //MergeEntityChanges(saveHandle.AuditLog);
    }

    private void ExecutePostContributors(AuditLogInfo auditLogInfo)
    {
        using var scope = serviceProvider.CreateScope();
        var context = new AuditLogContributionContext(scope.ServiceProvider, auditLogInfo);

        var options = scope.ServiceProvider.GetRequiredService<IOptions<FakeAuditingOptions>>().Value;

        foreach (var contributor in options.Contributors)
        {
            try
            {
                contributor.PostContribute(context);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, LogLevel.Warning);
            }
        }
    }


    protected class AuditLogSaveHandle(
        AuditingManager auditingManager,
        IDisposable scope,
        AuditLogInfo auditLog,
        Stopwatch stopWatch)
        : IAuditLogSaveHandle
    {
        public AuditLogInfo AuditLog { get; } = auditLog;
        public Stopwatch StopWatch { get; } = stopWatch;


        public async Task SaveAsync()
        {
            await auditingManager.SaveAsync(this);
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}