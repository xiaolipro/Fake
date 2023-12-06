using System;

namespace Fake.Consul;

/// <summary>
/// Consul服务连接配置
/// </summary>
public class FakeConsulClientOptions
{
    /// <summary>
    /// Consul地址
    /// </summary>
    public Uri Address { get; set; } = new Uri("http://localhost:8500");

    /// <summary>
    /// 数据中心
    /// </summary>
    public string Datacenter { get; set; } = "dc1";

    /// <summary>
    /// 令牌
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// 配置文件名称（完整的Key）
    /// </summary>
    public string? ConfigFileName { get; set; }
}