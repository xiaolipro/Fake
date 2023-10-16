
using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Events;

namespace Domain.Events
{
    public class OrderShippedDomainEvent : DomainEvent
    {
        public Order Order { get; }

        public OrderShippedDomainEvent(Order order)
        {
            Order = order;           
        }
    }
}
