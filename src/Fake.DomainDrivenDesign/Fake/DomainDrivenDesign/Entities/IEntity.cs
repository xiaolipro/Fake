using Fake.DomainDrivenDesign.Events;

namespace Fake.DomainDrivenDesign.Entities;

/// <summary>
/// 实体顶层抽象
/// 支持复合索引，单主键建议使用<see cref="IEntity{TKey}"/>
/// </summary>
public interface IEntity : IHasDomainEvent
{
    /// <summary>
    /// 获取所有主键
    /// </summary>
    /// <returns></returns>
    object[] GetKeys();

    /// <summary>
    /// 是否为临时实体，表示还未持久化
    /// </summary>
    bool IsTransient { get; }
}

/// <summary>
/// 实体顶层泛型抽象
/// </summary>
/// <typeparam name="TKey">唯一主键类型</typeparam>
// ReSharper disable once TypeParameterCanBeVariant
public interface IEntity<TKey> : IEntity
{
    /// <summary>
    /// 实体唯一标识
    /// </summary>
    TKey Id { get; }
}