using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public interface IUnitOfWork : IDatabaseApiContainer, ITransactionApiContainer, IDisposable
{
    public Guid Id { get; }
    
    UnitOfWorkContext Context { get; }

    IUnitOfWork Outer { get; }

    void InitUnitOfWorkContext(UnitOfWorkAttribute context);

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
    /// 完成所有操作（包括数据保存、事务提交）
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="FakeException">方法已被调用</exception>
    Task CompleteAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 事务提交后执行
    /// </summary>
    /// <param name="func"></param>
    void OnCompleted(Func<IUnitOfWork, Task> func);
    
    event EventHandler<UnitOfWorkCommitFailedEventArgs> CommitFailed;
    event EventHandler<UnitOfWorkEventArgs> Disposed;
    
    /// <summary>
    /// 设置外层工作单元
    /// </summary>
    /// <param name="outer"></param>
    void SetOuter([CanBeNull] IUnitOfWork outer);
}