using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Fake.Domain.Entities;

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