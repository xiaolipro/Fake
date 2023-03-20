using Domain.Aggregates.BuyerAggregate;
using Fake.Domain.Entities.Events;

namespace Domain.Events;

public class BuyerAndPaymentMethodVerifiedDomainEvent
    : DomainEvent
{
    public Buyer Buyer { get; private set; }
    public PaymentMethod Payment { get; private set; }
    public int OrderId { get; private set; }

    public BuyerAndPaymentMethodVerifiedDomainEvent(Buyer buyer, PaymentMethod payment, int orderId)
    {
        Buyer = buyer;
        Payment = payment;
        OrderId = orderId;
    }
}
