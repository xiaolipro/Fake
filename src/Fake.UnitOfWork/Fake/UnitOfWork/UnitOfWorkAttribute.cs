using System;
using System.Data;

namespace Fake.UnitOfWork;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
public class UnitOfWorkAttribute : Attribute
{
    /// <summary>
    /// 是否具有事务性
    /// </summary>
    public bool IsTransactional { get; set; }

    /// <summary>
    /// 事务级别
    /// </summary>
    public IsolationLevel IsolationLevel { get; set; }

    /// <summary>
    /// 超时时间
    /// </summary>
    public int Timeout { get; set; }
    
    /// <summary>
    /// 启用新的工作单元
    /// </summary>
    public bool RequiresNew { get; set; }
}