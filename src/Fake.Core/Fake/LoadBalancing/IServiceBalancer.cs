using System.Collections.Concurrent;

namespace Fake.LoadBalancing;

/// <summary>
/// 客户端负载均衡器
/// </summary>
public interface IServiceBalancer: IServiceResolver
{
    /// <summary>
    /// 执行均衡算法，返回一个节点
    /// </summary>
    /// <param name="serviceName"></param>
    /// <param name="isGrpc"></param>
    /// <returns></returns>
    string Pick(string serviceName, bool isGrpc = false);
}