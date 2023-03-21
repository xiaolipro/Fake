using Fake.Domain.Entities;
using Fake.UnitOfWork;

namespace Fake.Domain.Repositories;

/// <summary>
/// 仓储，专注于聚合的持久化
/// </summary>
/// <typeparam name="TAggregateRoot">聚合根</typeparam>
public interface IRepository<TAggregateRoot> where TAggregateRoot : IAggregateRoot
{
    //IUnitOfWork UnitOfWork { get; }
}

/// <summary>
/// 仓储，关注于单一聚合的持久化
/// </summary>
/// <typeparam name="TAggregateRoot">聚合根</typeparam>
/// <typeparam name="TKey">主键类型</typeparam>
public interface IRepository<TAggregateRoot, TKey> : IRepository<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot<TKey>
{
}