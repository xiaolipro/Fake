using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Fake.DependencyInjection;
using Fake.Threading;

namespace Fake.Auditing;

public class AuditingManager : IAuditingManager, ITransientDependency
{
    private const string AuditingContextKey = "Fake.Auditing.AuditLogScope";

    private readonly IScopeProvider<AuditLogScope> _scopeProvider;
    private readonly IAuditingHelper _auditingHelper;
    private readonly IAuditingStore _auditingStore;

    public AuditingManager(
        IScopeProvider<AuditLogScope> scopeProvider,
        IAuditingHelper auditingHelper,
        IAuditingStore auditingStore)
    {
        _scopeProvider = scopeProvider;
        _auditingHelper = auditingHelper;
        _auditingStore = auditingStore;
    }

    public AuditLogScope Current => _scopeProvider.GetContext(AuditingContextKey);

    public IAuditLogSaveHandle BeginScope()
    {
        var value = new AuditLogScope(_auditingHelper.CreateAuditLogInfo());
        var scope = _scopeProvider.BeginScope(AuditingContextKey,value);

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
        //ExecutePostContributors(saveHandle.AuditLog);
        //MergeEntityChanges(saveHandle.AuditLog);
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