using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Fake.Data;

namespace Fake.AspNetCore.LoadBalancing;

/// <summary>
/// 负载均衡器
/// </summary>
public interface ILoadBalancer : IHasExtraProperties
{
    /// <summary>
    /// 服务地址集合
    /// </summary>
    public List<string> ServiceAddresses { get; }

    /// <summary>
    /// 解析服务，得到服务地址集合
    /// </summary>
    /// <param name="service"></param>
    /// <returns></returns>
    Task ResolutionAsync(string service);

    /// <summary>
    /// 执行均衡算法，得到一个均衡地址
    /// </summary>
    /// <returns></returns>
    string Balancing();
}