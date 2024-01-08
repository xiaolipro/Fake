using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fake.UnitOfWork;

public interface IUnitOfWork : IDatabaseApiContainer, ITransactionApiContainer, IDisposable
{
    public Guid Id { get; }

    UnitOfWorkContext Context { get; }

    public bool IsDisposed { get; }

    public bool IsCompleted { get; }

    IUnitOfWork? Outer { get; }

    void InitUnitOfWorkContext(UnitOfWorkAttribute? context);

    /// <summary>
    /// 回滚事务
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RollbackAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 保存变更到数据库
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 是否有发生Change
    /// </summary>
    /// <returns></returns>
    bool HasHasChanges();

    /// <summary>
    /// 完成所有操作（包括数据保存、事务提交）
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="FakeException">方法已被调用</exception>
    Task CompleteAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 事务提交执行
    /// </summary>
    /// <param name="func"></param>
    void OnCompleted(Func<Task> func);

    /// <summary>
    /// 工作单元销毁触发该事件，但早于Disposed event
    /// 触发标准是<see cref="CompleteAsync"/>生命周期内发生异常
    /// </summary>
    event EventHandler<UnitOfWorkFailedEventArgs> Failed;

    event EventHandler<UnitOfWorkEventArgs> Disposed;

    /// <summary>
    /// 设置外层工作单元
    /// </summary>
    /// <param name="outer"></param>
    void SetOuter(IUnitOfWork? outer);
}