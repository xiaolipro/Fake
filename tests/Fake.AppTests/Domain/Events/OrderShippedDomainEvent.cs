using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Events;

namespace Domain.Events
{
    public class OrderShippedDomainEvent : DomainEvent
    {
#pragma warning disable CS0108, CS0114
        public Order Order { get; }
#pragma warning restore CS0108, CS0114

        public OrderShippedDomainEvent(Order order)
        {
            Order = order;
        }
    }
}