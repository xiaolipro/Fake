using System.Data;

namespace Fake.UnitOfWork;

public class FakeUnitOfWorkOptions:IClonable<FakeUnitOfWorkOptions>
{
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
    
    public FakeUnitOfWorkOptions(bool isTransactional = false, IsolationLevel isolationLevel = IsolationLevel.RepeatableRead, int timeout = -1)
    {
        IsTransactional = isTransactional;
        IsolationLevel = isolationLevel;
        Timeout = timeout;
    }

    public FakeUnitOfWorkOptions Clone()
    {
        return new FakeUnitOfWorkOptions
        {
            IsTransactional = IsTransactional,
            IsolationLevel = IsolationLevel,
            Timeout = Timeout
        };
    }
}