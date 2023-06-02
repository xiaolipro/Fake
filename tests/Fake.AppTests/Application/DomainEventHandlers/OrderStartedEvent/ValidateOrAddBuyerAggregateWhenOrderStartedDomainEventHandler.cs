using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Aggregates.BuyerAggregate;
using Domain.Events;
using Fake.EventBus.Events;
using Microsoft.Extensions.Logging;

namespace Application.DomainEventHandlers.OrderStartedDomainEventHandlers;

public class ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler : IEventHandler<OrderStartedDomainEvent>
{
    private readonly ILogger<ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler> _logger;
    private readonly IBuyerRepository _buyerRepository;

    public ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler(ILogger<ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler> logger,
        IBuyerRepository buyerRepository)
    {
        _logger = logger;
        _buyerRepository = buyerRepository;
    }

    public async Task Handle(OrderStartedDomainEvent orderStartedEvent, CancellationToken cancellationToken)
    {
        var cardTypeId = (orderStartedEvent.CardTypeId != 0) ? orderStartedEvent.CardTypeId : 1;
        var buyer = await _buyerRepository.FindAsync(orderStartedEvent.UserId);
        bool buyerOriginallyExisted = buyer != null;

        if (!buyerOriginallyExisted)
        {
            buyer = new Buyer(orderStartedEvent.UserId, orderStartedEvent.UserName);
        }

        buyer.AddPaymentMethod(cardTypeId,
            $"Payment Method on {DateTime.UtcNow}",
            orderStartedEvent.CardNumber,
            orderStartedEvent.CardSecurityNumber,
            orderStartedEvent.CardHolderName,
            orderStartedEvent.CardExpiration,
            orderStartedEvent.Order.Id);

        var buyerUpdated = buyerOriginallyExisted ? _buyerRepository.Update(buyer) : _buyerRepository.Add(buyer);

        //await _buyerRepository.UnitOfWork.CompleteAsync(cancellationToken);

        _logger.LogTrace("Buyer {BuyerId} and related payment method were validated or updated for orderId: {OrderId}.",
            buyerUpdated.Id, orderStartedEvent.Order.Id);
    }
}