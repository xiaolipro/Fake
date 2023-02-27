using System;
using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public class UnitOfWorkFailedEventArgs : UnitOfWorkEventArgs
{
    public Exception Exception { get; set; }

    public UnitOfWorkFailedEventArgs([NotNull] IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}