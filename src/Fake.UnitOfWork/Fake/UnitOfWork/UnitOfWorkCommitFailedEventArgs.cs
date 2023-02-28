using System;
using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public class UnitOfWorkCommitFailedEventArgs : UnitOfWorkEventArgs
{
    public Exception Exception { get; set; }

    public UnitOfWorkCommitFailedEventArgs([NotNull] IUnitOfWork unitOfWork, [NotNull] Exception exception) : base(unitOfWork)
    {
        Exception = exception;
    }
}