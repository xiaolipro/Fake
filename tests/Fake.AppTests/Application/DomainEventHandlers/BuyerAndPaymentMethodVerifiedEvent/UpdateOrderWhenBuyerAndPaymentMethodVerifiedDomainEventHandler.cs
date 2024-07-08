﻿using Domain.Aggregates.OrderAggregate;
using Domain.Events;
using Fake;
using Fake.Auditing;
using Fake.EventBus;

namespace Application.DomainEventHandlers.BuyerAndPaymentMethodVerifiedEvent;

public class
    UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler(
        IOrderRepository orderRepository,
        ILogger<UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler> logger)
    : IEventHandler<
        BuyerAndPaymentMethodVerifiedDomainEvent>
{
    public int Order { get; set; }

    [Audited]
    // Domain Logic comment:
    // When the Buyer and Buyer's payment method have been created or verified that they existed, 
    // then we can update the original Order with the BuyerId and PaymentId (foreign keys)
    public async Task HandleAsync(BuyerAndPaymentMethodVerifiedDomainEvent buyerPaymentMethodVerifiedEvent,
        CancellationToken cancellationToken)
    {
        var orderToUpdate = await orderRepository.GetAsync(buyerPaymentMethodVerifiedEvent.OrderId);
        ThrowHelper.ThrowIfNull(orderToUpdate, nameof(orderToUpdate));
        orderToUpdate!.SetBuyerId(buyerPaymentMethodVerifiedEvent.Buyer.Id);
        orderToUpdate.SetPaymentId(buyerPaymentMethodVerifiedEvent.Payment.Id);

        logger.LogDebug(
            "Order with Id: {OrderId} has been successfully updated with a payment method {PaymentMethod} ({Id})",
            buyerPaymentMethodVerifiedEvent.OrderId, nameof(buyerPaymentMethodVerifiedEvent.Payment),
            buyerPaymentMethodVerifiedEvent.Payment.Id);
    }
}