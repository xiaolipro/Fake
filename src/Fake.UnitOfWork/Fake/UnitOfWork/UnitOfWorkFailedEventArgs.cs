using System;
using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public class UnitOfWorkFailedEventArgs : UnitOfWorkEventArgs
{
    public Exception Exception { get; set; }
    public bool IsRollBacked { get; set; }

    public UnitOfWorkFailedEventArgs([NotNull] IUnitOfWork unitOfWork, [NotNull] Exception exception, bool isRollBacked) : base(unitOfWork)
    {
        Exception = exception;
        IsRollBacked = isRollBacked;
    }
}