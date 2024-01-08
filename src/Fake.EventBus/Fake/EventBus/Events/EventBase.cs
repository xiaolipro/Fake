using System;

namespace Fake.EventBus.Events;

public abstract class EventBase : IEvent
{
    /// <summary>
    /// 事件Id
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// 执行顺序（升序）
    /// </summary>
    public virtual long Order => 1;

    /// <summary>
    /// 事件创建时间
    /// </summary>
    public virtual DateTime CreationTime { get; } = DateTime.UtcNow;

    public override string ToString()
    {
        return $"[事件：{GetType().Name} Id：{Id} 创建时间：{CreationTime}]";
    }
}