using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Events;

namespace Domain.Events
{
    public class OrderCancelledDomainEvent : DomainEvent
    {
        public Order Order { get; }

        public OrderCancelledDomainEvent(Order order)
        {
            Order = order;
        }
    }
}
