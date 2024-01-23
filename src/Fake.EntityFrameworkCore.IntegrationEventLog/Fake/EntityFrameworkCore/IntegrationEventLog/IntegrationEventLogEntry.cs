using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using Fake.EventBus.Events;

namespace Fake.EntityFrameworkCore.IntegrationEventLog;

public class IntegrationEventLogEntry(EventBase @event, Guid transactionId)
{
    public Guid EventId { get; private set; } = @event.Id;

    public string EventTypeName { get; private set; } = @event.GetType().FullName ?? String.Empty;

    /// <summary>
    /// 事件状态
    /// </summary>
    public EventStateEnum State { get; private set; } = EventStateEnum.NotPublished;

    /// <summary>
    /// 发送次数
    /// </summary>
    public int TimesSent { get; private set; } = 0;

    /// <summary>
    /// 事件创建时间
    /// </summary>
    public DateTime CreationTime { get; private set; } = @event.CreationTime;

    /// <summary>
    /// 发送内容
    /// </summary>
    public string Content { get; private set; } = JsonSerializer.Serialize(@event);

    /// <summary>
    /// 事务Id
    /// </summary>
    public string TransactionId { get; private set; } = transactionId.ToString();

    [NotMapped] public string EventTypeShortName => EventTypeName.Split('.').Last();
    [NotMapped] public EventBase? IntegrationEvent { get; private set; }


    public IntegrationEventLogEntry DeserializeJsonContent(Type type)
    {
        IntegrationEvent = JsonSerializer.Deserialize(Content, type)?.As<EventBase>();
        return this;
    }

    public void UpdateEventStatus(EventStateEnum status)
    {
        State = status;
    }

    public void TimesSentIncr(int value = 1)
    {
        TimesSent += value;
    }
}