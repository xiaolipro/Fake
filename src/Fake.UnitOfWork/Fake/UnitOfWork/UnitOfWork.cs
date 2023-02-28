using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Fake.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fake.UnitOfWork;

public class UnitOfWork : IUnitOfWork, ITransientDependency
{
    public Guid Id { get; }
    
    public IUnitOfWork Outer { get; private set; }

    public UnitOfWorkStatus UnitOfWorkStatus { get; private set; }

    protected List<Func<IUnitOfWork, Task>> CompletedTasks { get; }
    protected List<Func<IUnitOfWork, Exception, Task>> CommitFailedTasks { get; }
    protected List<Func<IUnitOfWork, Task>> DisposedTasks { get; }

    protected virtual bool IsRollBack => UnitOfWorkStatus is UnitOfWorkStatus.RollBacking or UnitOfWorkStatus.RollBacked;
    protected virtual bool IsDispose => UnitOfWorkStatus is UnitOfWorkStatus.Disposing or UnitOfWorkStatus.Disposed;
    protected virtual bool IsComplete => UnitOfWorkStatus is UnitOfWorkStatus.Completing or UnitOfWorkStatus.Completed;

    private readonly Dictionary<string, IDatabaseApi> _databaseApiDic;
    private readonly Dictionary<string, ITransactionApi> _transactionApiDic;

    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(ILogger<UnitOfWork> logger)
    {
        _logger = logger;
        
        Id = Guid.NewGuid();
        
        CompletedTasks = new List<Func<IUnitOfWork, Task>>();
        CommitFailedTasks = new List<Func<IUnitOfWork, Exception, Task>>();
        DisposedTasks = new List<Func<IUnitOfWork, Task>>();
        
        _databaseApiDic = new Dictionary<string, IDatabaseApi>();
        _transactionApiDic = new Dictionary<string, ITransactionApi>();
    }

    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (IsRollBack)
        {
            return;
        }

        UnitOfWorkStatus = UnitOfWorkStatus.Saving;

        foreach (var databaseApi in GetAllActiveDatabaseApis())
        {
            if (databaseApi is ISupportSaveChanges supportSaveChanges)
            {
                try
                {
                    await supportSaveChanges.SaveChangesAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    UnitOfWorkStatus = UnitOfWorkStatus.SaveFailed;
                    _logger.LogException(e);
                    throw;
                }
            }
        }

        UnitOfWorkStatus = UnitOfWorkStatus.Saved;
    }

    public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (IsRollBack)
        {
            return;
        }
        
        UnitOfWorkStatus = UnitOfWorkStatus.RollBacking;

        foreach (var databaseApi in GetAllActiveDatabaseApis())
        {
            if (databaseApi is ISupportRollback supportRollback)
            {
                try
                {
                    await supportRollback.RollbackAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    UnitOfWorkStatus = UnitOfWorkStatus.RollBackFailed;
                    _logger.LogException(e);
                    throw;
                }
            }
        }

        UnitOfWorkStatus = UnitOfWorkStatus.RollBacked;
    }

    public virtual async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        if (IsRollBack)
        {
            return;
        }

        // 抑制多提交
        if (IsComplete)
        {
            throw new FakeException("方法已被调用");
        }

        UnitOfWorkStatus = UnitOfWorkStatus.Completing;

        try
        {
            await SaveChangesAsync(cancellationToken);

            foreach (var transaction in GetAllActiveTransactionApis())
            {
                await transaction.CommitAsync(cancellationToken);
            }

            await HandleCompletedTasksAsync();
        }
        catch (Exception ex)
        {
            UnitOfWorkStatus = UnitOfWorkStatus.CompletedFailed;
            await HandleCommitFailedTasksAsync(ex);
            throw;
        }

        UnitOfWorkStatus = UnitOfWorkStatus.Completed;
    }

    public virtual void OnCompleted(Func<IUnitOfWork, Task> func)
    {
        CompletedTasks.Add(func);
    }

    protected virtual async Task HandleCompletedTasksAsync()
    {
        CompletedTasks.Reverse();
        foreach (var task in CompletedTasks)
        {
            await task.Invoke(this);
        }
    }

    public virtual void OnCommitFailed(Func<IUnitOfWork,Exception, Task> func)
    {
        CommitFailedTasks.Add(func);
    }

    protected virtual async Task HandleCommitFailedTasksAsync(Exception ex)
    {
        CommitFailedTasks.Reverse();
        foreach (var task in CommitFailedTasks)
        {
            await task.Invoke(this, ex);
        }
    }

    public virtual void OnDisposed(Func<IUnitOfWork, Task> func)
    {
        DisposedTasks.Add(func);
    }

    public void SetOuter(IUnitOfWork outer)
    {
        Outer = outer;
    }

    protected virtual async Task HandleDisposedTasksAsync()
    {
        DisposedTasks.Reverse();
        foreach (var task in DisposedTasks)
        {
            await task.Invoke(this);
        }
    }

    public virtual IReadOnlyList<IDatabaseApi> GetAllActiveDatabaseApis()
    {
        return _databaseApiDic.Values.ToImmutableList();
    }

    public virtual IReadOnlyList<ITransactionApi> GetAllActiveTransactionApis()
    {
        return _transactionApiDic.Values.ToImmutableList();
    }

    public virtual async ValueTask DisposeAsync()
    {
        if (IsDispose)
        {
            return;
        }

        UnitOfWorkStatus = UnitOfWorkStatus.Disposing;

        // 工作单元销毁时一定要销毁所有事务防止数据库锁死
        foreach (var transactionApi in GetAllActiveTransactionApis())
        {
            try
            {
                transactionApi.Dispose();
            }
            catch (Exception e)
            {
                UnitOfWorkStatus = UnitOfWorkStatus.DisposeFailed;
                _logger.LogException(e);
            }
        }

        await HandleDisposedTasksAsync();
        UnitOfWorkStatus = UnitOfWorkStatus.Disposed;
    }

    public virtual IDatabaseApi FindDatabaseApi(string key)
    {
        return _databaseApiDic.GetOrDefault(key);
    }

    public virtual void AddDatabaseApi(string key, IDatabaseApi api)
    {
        ThrowHelper.ThrowIfNull(key, nameof(key));
        ThrowHelper.ThrowIfNull(api, nameof(api));

        if (_databaseApiDic.ContainsKey(key))
        {
            throw new FakeException($"此工作单元{this}中已存在具有给定密钥{key}的数据库API： ");
        }

        _databaseApiDic.Add(key, api);
    }

    public virtual IDatabaseApi GetOrAddDatabaseApi(string key, Func<IDatabaseApi> factory)
    {
        ThrowHelper.ThrowIfNull(key, nameof(key));
        ThrowHelper.ThrowIfNull(factory, nameof(factory));

        return _databaseApiDic.GetOrAdd(key, factory);
    }

    public override string ToString()
    {
        return $"[UnitOfWork {Id}]";
    }
}