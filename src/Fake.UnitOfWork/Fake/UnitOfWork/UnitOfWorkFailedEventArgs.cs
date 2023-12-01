using System;

namespace Fake.UnitOfWork;

public class UnitOfWorkFailedEventArgs : UnitOfWorkEventArgs
{
    public Exception Exception { get; set; }
    public bool IsRollBacked { get; set; }

    public UnitOfWorkFailedEventArgs(IUnitOfWork unitOfWork, Exception exception, bool isRollBacked) : base(unitOfWork)
    {
        Exception = exception;
        IsRollBacked = isRollBacked;
    }
}