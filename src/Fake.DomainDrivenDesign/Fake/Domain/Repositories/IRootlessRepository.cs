namespace Fake.Domain.Repositories;

/// <summary>
/// 无根仓储，面向数据库上下文操作
/// </summary>
public interface IRootlessRepository : IRepository, IReadOnlyUnitOfWorkEnabled
{
}