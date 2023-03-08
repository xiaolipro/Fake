using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Fake.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Fake.Auditing;

public class AuditingManager : IAuditingManager
{
    private const string AuditingContextKey = "Fake.Auditing.AuditLogScope";

    private readonly ILogger<AuditingManager> _logger;
    private readonly IAmbientScopeProvider<IAuditLogScope> _ambientScopeProvider;
    private readonly IAuditingHelper _auditingHelper;
    private readonly IAuditingStore _auditingStore;
    private readonly IServiceProvider _serviceProvider;
    

    public AuditingManager(
        IAmbientScopeProvider<IAuditLogScope> ambientScopeProvider,
        IAuditingHelper auditingHelper,
        IAuditingStore auditingStore,
        IServiceProvider serviceProvider)
    {
        _logger = NullLogger<AuditingManager>.Instance;
        _ambientScopeProvider = ambientScopeProvider;
        _auditingHelper = auditingHelper;
        _auditingStore = auditingStore;
        _serviceProvider = serviceProvider;
    }

    public IAuditLogScope Current => _ambientScopeProvider.GetValue(AuditingContextKey);

    public IAuditLogSaveHandle BeginScope()
    {
        var value = new AuditLogScope(_auditingHelper.CreateAuditLogInfo());
        var scope = _ambientScopeProvider.BeginScope(AuditingContextKey, value);

        Debug.Assert(Current != null, nameof(Current) + " != null");
        return new AuditLogSaveHandle(this, scope, Current.Log, Stopwatch.StartNew());
    }

    protected virtual async Task SaveAsync(AuditLogSaveHandle saveHandle)
    {
        BeforeSave(saveHandle);
        await _auditingStore.SaveAsync(saveHandle.AuditLog);
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
        using (var scope = _serviceProvider.CreateScope())
        {
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
    }


    protected class AuditLogSaveHandle : IAuditLogSaveHandle
    {
        private readonly AuditingManager _auditingManager;
        private readonly IDisposable _scope;
        public AuditLogInfo AuditLog { get; }
        public Stopwatch StopWatch { get; }

        public AuditLogSaveHandle(AuditingManager auditingManager, IDisposable scope, AuditLogInfo auditLog,
            Stopwatch stopWatch)
        {
            _auditingManager = auditingManager;
            _scope = scope;

            AuditLog = auditLog;
            StopWatch = stopWatch;
        }


        public async Task SaveAsync()
        {
            await _auditingManager.SaveAsync(this);
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}