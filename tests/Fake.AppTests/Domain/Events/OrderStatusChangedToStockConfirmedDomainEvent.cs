using Fake.Domain.Events;

namespace Domain.Events;

/// <summary>
/// Event used when the order stock items are confirmed
/// </summary>
public class OrderStatusChangedToStockConfirmedDomainEvent : DomainEvent
{
    public Guid OrderId { get; }

    public OrderStatusChangedToStockConfirmedDomainEvent(Guid orderId)
        => OrderId = orderId;
}