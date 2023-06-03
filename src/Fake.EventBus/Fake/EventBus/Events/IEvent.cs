using System;

namespace Fake.EventBus.Events;

/// <summary>
/// 事件
/// </summary>
public interface IEvent
{
    /// <summary>
    /// 唯一标识
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; }
}