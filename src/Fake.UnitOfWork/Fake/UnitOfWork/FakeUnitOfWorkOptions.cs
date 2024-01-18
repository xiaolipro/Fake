using System;
using System.Data;

namespace Fake.UnitOfWork;

/// <summary>
/// 工作单元默认配置，实际控制是<see cref="UnitOfWorkAttribute"/>
/// </summary>
public class FakeUnitOfWorkOptions
{
    /// <summary>
    /// 事务状态
    /// </summary>
    public UnitOfWorkTransactionState TransactionState { get; set; } = UnitOfWorkTransactionState.Auto; // 自动分析

    /// <summary>
    /// 事务级别
    /// </summary>
    public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadCommitted; // 读已提交

    /// <summary>
    /// 超时时间
    /// </summary>
    public int Timeout { get; set; } = -1; // 无限制

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