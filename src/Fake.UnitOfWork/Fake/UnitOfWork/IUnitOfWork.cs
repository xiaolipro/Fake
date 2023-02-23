using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public interface IUnitOfWork
{
    public Guid Id { get; }
    
    bool IsDisposed { get; }

    bool IsCompleted { get; }

    public event EventHandler<UnitOfWorkEventArgs> DisposedEvent;

    public event EventHandler<UnitOfWorkFailedEventArgs> FailedEvent;
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

    public UnitOfWorkEventArgs([NotNull]IUnitOfWork unitOfWork)
    {
        ThrowHelper.ThrowIfNull(unitOfWork);
        
        UnitOfWork = unitOfWork;
    }
}