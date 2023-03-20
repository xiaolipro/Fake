using System;

namespace Fake.EventBus.Events;

public abstract class EventBase:IEvent
{
    /// <summary>
    /// 唯一标识
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; }
        
    public EventBase()
    {
        Id = Guid.NewGuid();
        CreationTime = DateTime.UtcNow;
    }

    public override string ToString()
    {
        return $"[事件：{GetType().Name} Id：{Id} 创建时间：{CreationTime}]";
    }
}