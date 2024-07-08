using Domain.Aggregates.BuyerAggregate;
using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Events;

namespace Domain.Events
{
    /// <summary>
    /// Event used when an order is created
    /// </summary>
    public class OrderStartedDomainEvent : DomainEvent
    {
        public Guid UserId { get; }
        public string UserName { get; }
        public CardType CardType { get; }
        public string CardNumber { get; }
        public string CardSecurityNumber { get; }
        public string CardHolderName { get; }
        public DateTime CardExpiration { get; }
#pragma warning disable CS0108, CS0114
        public Order Order { get; }
#pragma warning restore CS0108, CS0114

        public OrderStartedDomainEvent(Order order, Guid userId, string userName,
            CardType cardType, string cardNumber,
            string cardSecurityNumber, string cardHolderName,
            DateTime cardExpiration)
        {
            Order = order;
            UserId = userId;
            UserName = userName;
            CardType = cardType;
            CardNumber = cardNumber;
            CardSecurityNumber = cardSecurityNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
        }
    }
}