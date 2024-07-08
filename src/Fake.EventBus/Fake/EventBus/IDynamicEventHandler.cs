using System.Threading.Tasks;

namespace Fake.EventBus
{
    /// <summary>
    /// 动态类型事件处理器
    /// </summary>
    public interface IDynamicEventHandler
    {
        /// <summary>
        /// 处理动态事件
        /// </summary>
        /// <param name="message">动态事件携带的数据</param>
        /// <returns></returns>
        Task Handle(string message);
    }
}