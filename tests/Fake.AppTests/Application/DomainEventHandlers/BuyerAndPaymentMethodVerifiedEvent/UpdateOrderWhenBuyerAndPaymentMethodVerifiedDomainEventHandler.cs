using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Aggregates.OrderAggregate;
using Domain.Events;
using Fake.EventBus.Events;
using Microsoft.Extensions.Logging;

namespace Application.DomainEventHandlers.BuyerAndPaymentMethodVerifiedEvent;

public class UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler:IEventHandler<BuyerAndPaymentMethodVerifiedDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler> _logger;

    public UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler(
        IOrderRepository orderRepository, ILogger<UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger;
    }

    // Domain Logic comment:
    // When the Buyer and Buyer's payment method have been created or verified that they existed, 
    // then we can update the original Order with the BuyerId and PaymentId (foreign keys)
    public async Task Handle(BuyerAndPaymentMethodVerifiedDomainEvent buyerPaymentMethodVerifiedEvent, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetAsync(buyerPaymentMethodVerifiedEvent.OrderId);
        orderToUpdate.SetBuyerId(buyerPaymentMethodVerifiedEvent.Buyer.Id);
        orderToUpdate.SetPaymentId(buyerPaymentMethodVerifiedEvent.Payment.Id);

        _logger.LogTrace("Order with Id: {OrderId} has been successfully updated with a payment method {PaymentMethod} ({Id})",
                buyerPaymentMethodVerifiedEvent.OrderId, nameof(buyerPaymentMethodVerifiedEvent.Payment), buyerPaymentMethodVerifiedEvent.Payment.Id);
    }
}