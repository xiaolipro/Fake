using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

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

public class UnitOfWorkFailedEventArgs : UnitOfWorkEventArgs
{
    public Exception Exception { get; set; }

    public UnitOfWorkFailedEventArgs([NotNull] IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}

public class UnitOfWorkEventArgs
{
    public IUnitOfWork UnitOfWork { get; }

    public UnitOfWorkEventArgs([NotNull] IUnitOfWork unitOfWork)
    {
        ThrowHelper.ThrowIfNull(unitOfWork);

        UnitOfWork = unitOfWork;
    }
}