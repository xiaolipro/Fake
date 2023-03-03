using System;
using System.Data;

namespace Fake.UnitOfWork;

/// <summary>
/// 工作单元默认配置，实际控制是<see cref="UnitOfWorkAttribute"/>
/// </summary>
public class FakeUnitOfWorkOptions
{
    /// <summary>
    /// 事务状态，默认：<see cref="UnitOfWorkTransactionState.Auto"/>
    /// </summary>
    public UnitOfWorkTransactionState TransactionState { get; set; }

    /// <summary>
    /// 事务级别，默认：RepeatableRead
    /// </summary>
    public IsolationLevel IsolationLevel { get; set; }

    /// <summary>
    /// 超时时间，默认：-1（无限制）
    /// </summary>
    public int Timeout { get; set; }

    public FakeUnitOfWorkOptions()
    {
        TransactionState = UnitOfWorkTransactionState.Auto;
        IsolationLevel = IsolationLevel.RepeatableRead;
        Timeout = -1;
    }

    public bool CalculateIsTransactional(bool autoValue)
    {
        switch (TransactionState)
        {
            case UnitOfWorkTransactionState.Auto:
                return autoValue;
            case UnitOfWorkTransactionState.Enable:
                return true;
            case UnitOfWorkTransactionState.Disable:
                return false;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}