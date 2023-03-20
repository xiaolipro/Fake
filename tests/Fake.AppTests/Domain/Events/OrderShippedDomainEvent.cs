
using Domain.Aggregations;
using Fake.Domain.Entities.Events;
using Fake.EventBus.Events;

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
