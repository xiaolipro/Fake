using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fake.UnitOfWork;

public interface IUnitOfWork : IDbContextContainer,IDisposable
{
    public Guid Id { get; }
    
    bool IsDisposed { get; }

    bool IsCompleted { get; }

    public event EventHandler<UnitOfWorkEventArgs> DisposedEvent;

    public event EventHandler<UnitOfWorkFailedEventArgs> FailedEvent;
    
    /// <summary>
    /// 保存变更
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 回滚
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RollbackAsync(CancellationToken cancellationToken = default);
}