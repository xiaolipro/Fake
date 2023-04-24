using System.Threading;
using System.Threading.Tasks;
using Fake.EventBus.Events;

namespace Fake.EventBus;

/// <summary>
/// 事件发布者--发布者模式
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// 发布事件
    /// </summary>
    /// <param name="event">事件</param>
    /// <param name="cancellationToken"></param>
    Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default);
}