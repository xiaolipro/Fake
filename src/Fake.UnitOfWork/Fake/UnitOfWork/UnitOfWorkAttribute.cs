using System;
using System.Data;

namespace Fake.UnitOfWork;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
public class UnitOfWorkAttribute : Attribute
{
    /// <summary>
    /// 启用工作单元，默认: true
    /// </summary>
    public bool IsEnabled { get; set; }
    
    /// <summary>
    /// 是否具有事务性，默认：false
    /// </summary>
    public bool IsTransactional { get; set; }

    /// <summary>
    /// 事务级别，默认：RepeatableRead（可重复读）
    /// </summary>
    public IsolationLevel IsolationLevel { get; set; }

    /// <summary>
    /// 超时时间，默认：-1（无限制）
    /// </summary>
    public int Timeout { get; set; }
    
    public UnitOfWorkAttribute()
    {
        IsEnabled = true;
        IsTransactional = false;
        IsolationLevel = IsolationLevel.RepeatableRead;
        Timeout = -1;
    }
}