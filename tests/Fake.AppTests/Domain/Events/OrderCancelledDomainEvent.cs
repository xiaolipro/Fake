using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Events;

namespace Domain.Events
{
    public class OrderCancelledDomainEvent : DomainEvent
    {
#pragma warning disable CS0108, CS0114
        public Order Order { get; }
#pragma warning restore CS0108, CS0114

        public OrderCancelledDomainEvent(Order order)
        {
            Order = order;
        }
    }
}