using Fake.Data;

namespace Fake.LoadBalancing;

/// <summary>
/// 负载均衡器
/// </summary>
public interface ILoadBalancer:IHasExtraProperties
{
    /// <summary>
    /// 均衡地址集合
    /// </summary>
    public List<string> BalancerAddresses { get; }
    
    /// <summary>
    /// 解析服务，得到均衡地址集合
    /// </summary>
    /// <param name="service"></param>
    /// <returns></returns>
    Task<List<string>> Resolution(string service);

    /// <summary>
    /// 执行均衡算法，得到一个均衡地址
    /// </summary>
    /// <returns></returns>
    string Pick();
}