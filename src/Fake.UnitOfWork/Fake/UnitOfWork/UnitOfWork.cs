using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fake.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    public Guid Id { get; }
    public bool IsDisposed { get; }
    public bool IsCompleted { get; }
    public event EventHandler<UnitOfWorkEventArgs> DisposedEvent;
    public event EventHandler<UnitOfWorkFailedEventArgs> FailedEvent;
    
    private bool _isRolledBack;

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_isRolledBack) return;
        _isRolledBack = true;
        
        await RollbackAllAsync(cancellationToken);
    }

    public Task<IDbContext> GetDbContextAsync()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

public interface ISupportRollback
{
    
}