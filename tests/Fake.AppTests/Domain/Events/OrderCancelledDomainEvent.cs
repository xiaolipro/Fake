using Domain.Aggregations;
using Fake.Domain.Entities.Events;

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
