using System.Data;

namespace Fake.UnitOfWork;

public class UnitOfWorkContext : ICloneable<UnitOfWorkContext>
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


    public UnitOfWorkContext Clone()
    {
        return new UnitOfWorkContext
        {
            IsTransactional = IsTransactional,
            IsolationLevel = IsolationLevel,
            Timeout = Timeout
        };
    }
}