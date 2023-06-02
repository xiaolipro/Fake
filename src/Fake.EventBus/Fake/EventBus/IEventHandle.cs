using System.Threading;
using System.Threading.Tasks;
using Fake.EventBus.Events;

namespace Fake.EventBus;

public interface IEventHandle<in TEvent> where TEvent:IEvent
{
    /// <summary>
    /// Handles a notification
    /// </summary>
    /// <param name="event">The notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task Handle(TEvent @event, CancellationToken cancellationToken);
}