using System;
using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Entities.Events;

namespace Domain.Events
{
    using System.Collections.Generic;

    /// <summary>
    /// Event used when the order is paid
    /// </summary>
    public class OrderStatusChangedToPaidDomainEvent
        : DomainEvent
    {
        public Guid OrderId { get; }
        public IEnumerable<OrderItem> OrderItems { get; }

        public OrderStatusChangedToPaidDomainEvent(Guid orderId,
            IEnumerable<OrderItem> orderItems)
        {
            OrderId = orderId;
            OrderItems = orderItems;
        }
    }
}