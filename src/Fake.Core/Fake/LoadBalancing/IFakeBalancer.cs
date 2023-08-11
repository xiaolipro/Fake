using System.Collections.Concurrent;

namespace Fake.LoadBalancing;

/// <summary>
/// 客户端负载均衡器
/// </summary>
public interface IFakeBalancer
{
    /// <summary>
    /// 服务节点
    /// </summary>
    ConcurrentDictionary<string, List<string>> ServiceNodes { get; }

    /// <summary>
    /// 刷新节点
    /// </summary>
    /// <param name="serviceName"></param>
    /// <returns></returns>
    Task ReferenceAsync(string serviceName);

    /// <summary>
    /// 执行均衡算法，返回一个节点
    /// </summary>
    /// <param name="serviceName"></param>
    /// <returns></returns>
    string Pick(string serviceName);
}