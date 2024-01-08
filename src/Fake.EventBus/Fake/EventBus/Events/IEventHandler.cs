using System.Threading;
using System.Threading.Tasks;

namespace Fake.EventBus.Events;

/// <summary>
/// 强类型事件处理器
/// </summary>
/// <typeparam name="TEvent">事件</typeparam>
public interface IEventHandler<in TEvent> where TEvent : EventBase
{
    /// <summary>
    /// 处理事件
    /// </summary>
    /// <param name="event">事件携带的数据</param>
    /// <param name="cancellationToken">任务取消令牌</param>
    /// <returns></returns>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}