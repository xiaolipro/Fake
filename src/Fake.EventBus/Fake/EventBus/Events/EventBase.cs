using System;

namespace Fake.EventBus.Events;

public abstract class EventBase:IEvent
{
    public Guid Id { get; }

    public int Order { get; set; }
    
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