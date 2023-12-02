using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using Fake.EventBus.Events;

namespace Fake.EntityFrameworkCore.IntegrationEventLog;

public class IntegrationEventLogEntry
{
    private IntegrationEventLogEntry()
    {
    }

    public IntegrationEventLogEntry(IEvent @event, Guid transactionId)
    {
        EventId = @event.Id;
        CreationTime = @event.CreationTime;
        EventTypeName = @event.GetType().FullName ?? String.Empty;
        Content = JsonSerializer.Serialize(@event);
        State = EventStateEnum.NotPublished;
        TimesSent = 0;
        TransactionId = transactionId.ToString();
    }

    public Guid EventId { get; private set; }

    public string EventTypeName { get; private set; }

    /// <summary>
    /// 事件状态
    /// </summary>
    public EventStateEnum State { get; private set; }

    /// <summary>
    /// 发送次数
    /// </summary>
    public int TimesSent { get; private set; }

    /// <summary>
    /// 事件创建时间
    /// </summary>
    public DateTime CreationTime { get; private set; }

    /// <summary>
    /// 发送内容
    /// </summary>
    public string Content { get; private set; }

    /// <summary>
    /// 事务Id
    /// </summary>
    public string TransactionId { get; private set; }

    [NotMapped] public string EventTypeShortName => EventTypeName.Split('.').Last();
    [NotMapped] public IEvent? IntegrationEvent { get; private set; }


    public IntegrationEventLogEntry DeserializeJsonContent(Type type)
    {
        IntegrationEvent = JsonSerializer.Deserialize(Content, type)?.As<IEvent>();
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