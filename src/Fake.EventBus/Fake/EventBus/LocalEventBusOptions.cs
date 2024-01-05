using System.Threading.Channels;

namespace Fake.EventBus;

public class LocalEventBusOptions
{
    /// <summary>
    /// 事件总线容量
    /// </summary>
    public int Capacity { get; set; } = 100;

    /// <summary>
    /// 总线满了后的处理方式
    /// </summary>
    public BoundedChannelFullMode FullMode { get; set; } = BoundedChannelFullMode.Wait;

    /// <summary>
    /// 消费线程数量
    /// </summary>
    public int ConsumerThreads { get; set; } = 1;
}