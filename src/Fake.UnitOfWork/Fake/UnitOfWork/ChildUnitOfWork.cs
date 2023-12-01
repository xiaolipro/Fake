using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fake.UnitOfWork;

public class ChildUnitOfWork : IUnitOfWork
{
    public IServiceProvider ServiceProvider => _parent.ServiceProvider;
    public Guid Id => _parent.Id;
    public UnitOfWorkContext Context => _parent.Context;
    public bool IsDisposed => _parent.IsDisposed;
    public bool IsCompleted => _parent.IsCompleted;
    public IUnitOfWork Outer => _parent.Outer;
    public event EventHandler<UnitOfWorkFailedEventArgs> Failed;
    public event EventHandler<UnitOfWorkEventArgs> Disposed;

    private readonly IUnitOfWork _parent;

    public ChildUnitOfWork(IUnitOfWork parent)
    {
        ThrowHelper.ThrowIfNull(parent, nameof(parent));

        _parent = parent;

        _parent.Failed += (sender, args) => { Failed?.Invoke(sender, args); };
        _parent.Disposed += (sender, args) => { Disposed?.Invoke(sender, args); };
    }


    public IDatabaseApi FindDatabaseApi(string key)
    {
        return _parent.FindDatabaseApi(key);
    }

    public void AddDatabaseApi(string key, IDatabaseApi api)
    {
        _parent.AddDatabaseApi(key, api);
    }

    public IDatabaseApi GetOrAddDatabaseApi(string key, Func<IDatabaseApi> factory)
    {
        return _parent.GetOrAddDatabaseApi(key, factory);
    }

    public ITransactionApi FindTransactionApi(string key)
    {
        return _parent.FindTransactionApi(key);
    }

    public void AddTransactionApi(string key, ITransactionApi api)
    {
        _parent.AddTransactionApi(key, api);
    }

    public ITransactionApi GetOrAddTransactionApi(string key, Func<ITransactionApi> factory)
    {
        return _parent.GetOrAddTransactionApi(key, factory);
    }

    public void Dispose()
    {
    }


    public void InitUnitOfWorkContext(UnitOfWorkAttribute context)
    {
        _parent.InitUnitOfWorkContext(context);
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        return _parent.RollbackAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _parent.SaveChangesAsync(cancellationToken);
    }

    public bool HasHasChanges()
    {
        return _parent.HasHasChanges();
    }

    public Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public void OnCompleted(Func<IUnitOfWork, Task> func)
    {
        _parent.OnCompleted(func);
    }

    public void SetOuter(IUnitOfWork outer)
    {
        _parent.SetOuter(outer);
    }

    public override string ToString()
    {
        return $"[UnitOfWork {Id}]";
    }
}