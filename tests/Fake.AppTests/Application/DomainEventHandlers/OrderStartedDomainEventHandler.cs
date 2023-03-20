using System.Threading;
using System.Threading.Tasks;
using Domain.Events;
using Fake.EventBus.Events;

namespace Application.DomainEventHandlers;

public class OrderStartedDomainEventHandler:IEventHandler<OrderStartedDomainEvent>
{
    public Task Handle(OrderStartedDomainEvent @event, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}