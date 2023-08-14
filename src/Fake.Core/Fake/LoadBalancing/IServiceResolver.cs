using System.Collections.Concurrent;

namespace Fake.LoadBalancing;

public interface IServiceResolver
{
    /// <summary>
    /// 服务节点
    /// </summary>
    ConcurrentDictionary<string, List<ServiceResolveResult>> ServiceNodes { get; }
    
    /// <summary>
    /// 解析服务
    /// </summary>
    /// <param name="serviceName"></param>
    /// <returns></returns>
    Task ResolutionAsync(string serviceName);
}