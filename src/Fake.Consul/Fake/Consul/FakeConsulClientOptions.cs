using JetBrains.Annotations;

namespace Fake.Consul;

public class FakeConsulClientOptions
{
    /// <summary>
    /// 服务地址
    /// </summary>
    public string Address { get; set; }
    
    /// <summary>
    /// 服务Grpc地址
    /// </summary>
    [CanBeNull]
    public string GrpcAddress { get; set; }
    
    /// <summary>
    /// 服务组名称
    /// </summary>
    public string ServiceName { get; set; }
    
    /// <summary>
    /// 服务标签
    /// </summary>
    [CanBeNull]
    public string[] Tags { get; set; }
    
    /// <summary>
    /// 服务心跳检查路径
    /// </summary>
    public string HealthCheckPath { get; set; }
    
    /// <summary>
    /// Grpc服务心跳检查路径
    /// </summary>
    [CanBeNull]
    public string GrpcHealthCheckPath { get; set; }
    
    /// <summary>
    /// 心跳检测间隔(s)
    /// </summary>
    public int Interval { get; set; }
    
    /// <summary>
    /// 心跳超时时间(s)
    /// </summary>
    public int Timeout { get; set; }
    
    /// <summary>
    /// 心跳停止多久后注销服务(s)
    /// </summary>
    public int DeregisterTime { get; set; }
    
    /// <summary>
    /// 权重(使用权重调度器时有效)
    /// </summary>
    public int Weight { get; set; }
}