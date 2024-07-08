using Domain.Aggregates.BuyerAggregate;
using Fake.Domain.Events;

namespace Domain.Events;

public class BuyerAndPaymentMethodVerifiedDomainEvent
    : DomainEvent
{
    public Buyer Buyer { get; private set; }
    public PaymentMethod Payment { get; private set; }
    public Guid OrderId { get; private set; }

    public BuyerAndPaymentMethodVerifiedDomainEvent(Buyer buyer, PaymentMethod payment, Guid orderId)
    {
        Buyer = buyer;
        Payment = payment;
        OrderId = orderId;
    }
}