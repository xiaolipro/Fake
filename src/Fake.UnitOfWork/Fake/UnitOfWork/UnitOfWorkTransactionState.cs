namespace Fake.UnitOfWork;

public enum UnitOfWorkTransactionState
{
    /// <summary>
    /// 自动分析场景，启停事务
    /// </summary>
    Auto,
    /// <summary>
    /// 全局启用事务
    /// </summary>
    Enable,
    /// <summary>
    /// 全局禁用事务
    /// </summary>
    Disable
}