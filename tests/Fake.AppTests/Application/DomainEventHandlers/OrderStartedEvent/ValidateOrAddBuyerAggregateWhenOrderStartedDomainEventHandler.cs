using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Aggregates.BuyerAggregate;
using Domain.Events;
using Fake.Domain.Repositories;
using Fake.EventBus.Events;
using Fake.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Application.DomainEventHandlers.OrderStartedEvent;

public class ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler : IEventHandler<OrderStartedDomainEvent>
{
    private readonly ILogger<ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler> _logger;
    private readonly IRepository<Buyer> _buyerRepository;

    public ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler(
        ILogger<ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler> logger,
        IBuyerRepository buyerRepository)
    {
        _logger = logger;
        _buyerRepository = buyerRepository;
    }

    public async Task Handle(OrderStartedDomainEvent orderStartedEvent, CancellationToken cancellationToken)
    {
        var buyer = await _buyerRepository.FirstOrDefaultAsync(x => x.IdentityGuid == orderStartedEvent.UserId,
            cancellationToken: cancellationToken);
        bool buyerOriginallyExisted = buyer != null;

        if (!buyerOriginallyExisted)
        {
            buyer = new Buyer(orderStartedEvent.UserId, orderStartedEvent.UserName);
        }

        buyer.AddPaymentMethod(orderStartedEvent.CardType,
            $"Payment Method on {DateTime.UtcNow}",
            orderStartedEvent.CardNumber,
            orderStartedEvent.CardSecurityNumber,
            orderStartedEvent.CardHolderName,
            orderStartedEvent.CardExpiration,
            orderStartedEvent.Order.Id);

        var buyerUpdated = buyerOriginallyExisted
            ? _buyerRepository.UpdateAsync(buyer, cancellationToken: cancellationToken)
            : _buyerRepository.InsertAsync(buyer, cancellationToken: cancellationToken);

        await _buyerRepository.UnitOfWork.CompleteAsync(cancellationToken);
        _logger.LogTrace("Buyer {BuyerId} and related payment method were validated or updated for orderId: {OrderId}.",
            buyerUpdated.Id, orderStartedEvent.Order.Id);
    }
}