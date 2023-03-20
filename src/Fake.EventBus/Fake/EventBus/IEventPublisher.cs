using System.Threading;
using System.Threading.Tasks;
using Fake.EventBus.Events;

namespace Fake.EventBus;

public interface IEventPublisher
{
    /// <summary>
    /// 发布事件
    /// </summary>
    /// <param name="event">事件</param>
    /// <param name="cancellationToken"></param>
    Task Publish(IEvent @event, CancellationToken cancellationToken = default);
}