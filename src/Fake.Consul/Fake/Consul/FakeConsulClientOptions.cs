using System;

namespace Fake.Consul;

/// <summary>
/// Consul服务连接配置
/// </summary>
public class FakeConsulClientOptions
{
    /// <summary>
    /// Consul地址
    /// ex：http://localhost:8500
    /// </summary>
    public Uri Address { get; set; }

    /// <summary>
    /// 数据中心
    /// ex：dc1
    /// </summary>
    public string Datacenter { get; set; }
    
    /// <summary>
    /// 令牌
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// 配置文件名称（完整的Key）
    /// </summary>
    public string ConfigFileName { get; set; }
}