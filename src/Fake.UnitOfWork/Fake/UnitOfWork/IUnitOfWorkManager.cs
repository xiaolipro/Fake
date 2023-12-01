namespace Fake.UnitOfWork;

public interface IUnitOfWorkManager
{
    /// <summary>
    /// 当前所处的工作单元
    /// </summary>
    IUnitOfWork? Current { get; }

    /// <summary>
    /// 开启一个工作单元
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    IUnitOfWork Begin(UnitOfWorkAttribute? attribute = default);
}