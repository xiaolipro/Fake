using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fake.EventBus;
using Fake.EventBus.Events;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fake.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    public Guid Id { get; }
    public List<IEvent> Events { get; }
    public IServiceProvider ServiceProvider { get; }
    public UnitOfWorkContext Context { get; private set; }

    public bool IsDisposed { get; private set; }

    public bool IsCompleted { get; private set; }
    public IUnitOfWork Outer { get; private set; }

    protected List<Func<IUnitOfWork, Task>> CompletedTasks { get; }
    public event EventHandler<UnitOfWorkFailedEventArgs> Failed;
    public event EventHandler<UnitOfWorkEventArgs> Disposed;

    private readonly Dictionary<string, IDatabaseApi> _databaseApiDic;
    private readonly Dictionary<string, ITransactionApi> _transactionApiDic;

    private readonly ILogger<UnitOfWork> _logger;
    private readonly IEventPublisher _eventPublisher;
    private readonly FakeUnitOfWorkOptions _options;

    private Exception _exception;
    private bool _isCompleting, _isRollBacked;

    public UnitOfWork(IServiceProvider serviceProvider, ILogger<UnitOfWork> logger,
        IOptions<FakeUnitOfWorkOptions> options, IEventPublisher eventPublisher)
    {
        ServiceProvider = serviceProvider;
        _logger = logger;
        _eventPublisher = eventPublisher;
        _options = options.Value;

        Id = Guid.NewGuid();

        CompletedTasks = new List<Func<IUnitOfWork, Task>>();

        _databaseApiDic = new Dictionary<string, IDatabaseApi>();
        _transactionApiDic = new Dictionary<string, ITransactionApi>();

        Events = new List<IEvent>();
    }

    /// <summary>
    /// 初始化工作单元上下文
    /// </summary>
    /// <param name="attribute"></param>
    /// <exception cref="FakeException"></exception>
    public virtual void InitUnitOfWorkContext([CanBeNull] UnitOfWorkAttribute attribute)
    {
        if (Context != null)
        {
            throw new FakeException($"{this}工作单元上下文已经被初始化过了!");
        }

        var context = new UnitOfWorkContext
        {
            // 优先使用UnitOfWorkAttribute的配置，然后才是全局默认配置
            IsolationLevel = attribute?.IsolationLevel ?? _options.IsolationLevel,
            Timeout = attribute?.Timeout ?? _options.Timeout,
            IsTransactional = attribute?.IsTransactional ?? _options.CalculateIsTransactional(
                autoValue: ServiceProvider.GetRequiredService<IUnitOfWorkTransactionalProvider>().IsTransactional
            )
        };

        Context = context;
    }

    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_isRollBacked)
        {
            _logger.LogWithLevel(LogLevel.Warning, $"{this}正在尝试{nameof(SaveChangesAsync)}一个已经回滚过的事务");
            return;
        }

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
                    _logger.LogException(e);
                    throw;
                }
            }
        }
    }

    public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_isRollBacked)
        {
            _logger.LogWithLevel(LogLevel.Warning, $"{this}正在尝试{nameof(RollbackAsync)}一个已经回滚过的事务");
            return;
        }

        _isRollBacked = true;

        foreach (var databaseApi in GetAllActiveDatabaseApis())
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (databaseApi is ISupportRollback supportRollback)
            {
                try
                {
                    await supportRollback.RollbackAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogException(e);
                    throw;
                }
            }
        }
    }

    public virtual async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        if (_isRollBacked)
        {
            _logger.LogWithLevel(LogLevel.Warning, $"{this}正在尝试{nameof(CompleteAsync)}一个已经回滚过的事务");
            return;
        }

        // 抑制多提交
        if (_isCompleting || IsCompleted)
        {
            throw new FakeException($"{nameof(CompleteAsync)}方法已被调用过了");
        }


        try
        {
            _isCompleting = true;
            await SaveChangesAsync(cancellationToken);
            
            DispatchEvents();

            await CommitTransactionsAsync(cancellationToken);

            IsCompleted = true;
            await HandleCompletedTasksAsync();
        }
        catch (Exception ex)
        {
            _exception = ex;
            throw;
        }
    }

    public void DispatchEvents()
    {
        if (Events.Any())
        {
            var toBePublished = Events.OrderBy(e => e.Order).ToArray();
            Events.Clear();
            foreach (var @event in toBePublished)
            {
                _eventPublisher.Publish(@event);
            }
        }
    }

    private async Task CommitTransactionsAsync(CancellationToken cancellationToken)
    {
        foreach (var transaction in GetAllActiveTransactionApis())
        {
            await transaction.CommitAsync(cancellationToken);
        }
    }

    protected virtual void OnFailed()
    {
        Failed?.Invoke(this, new UnitOfWorkFailedEventArgs(this, _exception, _isRollBacked));
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

    public virtual void OnDisposed()
    {
        Disposed?.Invoke(this, new UnitOfWorkEventArgs(this));
    }

    public void SetOuter(IUnitOfWork outer)
    {
        Outer = outer;
    }

    public virtual IReadOnlyList<IDatabaseApi> GetAllActiveDatabaseApis()
    {
        return _databaseApiDic.Values.ToImmutableList();
    }

    public virtual IReadOnlyList<ITransactionApi> GetAllActiveTransactionApis()
    {
        return _transactionApiDic.Values.ToImmutableList();
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
            throw new FakeException($"工作单元{this}中已存在具有给定密钥{key}的数据库API");
        }

        _databaseApiDic.Add(key, api);
    }

    public virtual IDatabaseApi GetOrAddDatabaseApi(string key, Func<IDatabaseApi> factory)
    {
        ThrowHelper.ThrowIfNull(key, nameof(key));
        ThrowHelper.ThrowIfNull(factory, nameof(factory));

        return _databaseApiDic.GetOrAdd(key, factory);
    }

    public ITransactionApi FindTransactionApi(string key)
    {
        return _transactionApiDic.GetOrDefault(key);
    }

    public void AddTransactionApi(string key, ITransactionApi api)
    {
        ThrowHelper.ThrowIfNull(key, nameof(key));
        ThrowHelper.ThrowIfNull(api, nameof(api));

        if (_transactionApiDic.ContainsKey(key))
        {
            throw new FakeException($"工作单元{this}中已存在具有给定密钥{key}的数据库API");
        }

        _transactionApiDic.Add(key, api);
    }

    public ITransactionApi GetOrAddTransactionApi(string key, Func<ITransactionApi> factory)
    {
        ThrowHelper.ThrowIfNull(key, nameof(key));
        ThrowHelper.ThrowIfNull(factory, nameof(factory));

        return _transactionApiDic.GetOrAdd(key, factory);
    }

    public override string ToString()
    {
        return $"[UnitOfWork {Id}]";
    }

    public void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        IsDisposed = true;

        // 工作单元销毁时一定要销毁所有事务防止数据库锁死
        foreach (var transactionApi in GetAllActiveTransactionApis())
        {
            try
            {
                transactionApi.Dispose();
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }

        if (!IsCompleted || _exception != null)
        {
            OnFailed();
        }

        OnDisposed();
    }
}