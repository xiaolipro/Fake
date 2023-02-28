using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fake.UnitOfWork;

public interface IUnitOfWork : IDatabaseApiContainer, IAsyncDisposable
{
    public Guid Id { get; }

    UnitOfWorkStatus UnitOfWorkStatus { get; }

    /// <summary>
    /// 保存变更
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 回滚事务
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RollbackAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 提交事务
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CompleteAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 事务提交后执行
    /// </summary>
    /// <param name="func"></param>
    void OnCompleted(Func<IUnitOfWork, Task> func);

    /// <summary>
    /// 事务提交失败执行
    /// </summary>
    /// <param name="func"></param>
    void OnCommitFailed(Func<IUnitOfWork, Exception, Task> func);

    /// <summary>
    /// 工作单元销毁时执行
    /// </summary>
    /// <param name="func"></param>
    void OnDisposed(Func<IUnitOfWork, Task> func);
}

public enum UnitOfWorkStatus
{
    Active,

    Saving,
    Saved,
    SaveFailed,

    /// <summary>
    /// 提交中
    /// </summary>
    Commiting,

    /// <summary>
    /// 已提交
    /// </summary>
    Committed,

    /// <summary>
    /// 提交失败
    /// </summary>
    CommitFailed,

    /// <summary>
    /// 回滚中
    /// </summary>
    RollBacking,

    /// <summary>
    /// 已回滚
    /// </summary>
    RollBacked,

    /// <summary>
    /// 回滚失败
    /// </summary>
    RollBackFailed,

    Disposing,
    Disposed,
    DisposeFailed
}