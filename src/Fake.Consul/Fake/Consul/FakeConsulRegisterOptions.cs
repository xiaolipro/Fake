namespace Fake.Consul;

public class FakeConsulRegisterOptions
{
    /// <summary>
    /// 主机
    /// ex：127.0.0.1
    /// </summary>
    public string Host { get; set; } = null!;

    /// <summary>
    /// 服务端口
    /// ex：8080
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Grpc端口
    /// </summary>
    public int GrpcPort { get; set; }

    /// <summary>
    /// 服务标签
    /// </summary>
    public string[]? Tags { get; set; } = null;

    /// <summary>
    /// 服务心跳检查路径
    /// </summary>
    public string HealthCheckPath { get; set; } = "/health";

    /// <summary>
    /// Grpc服务心跳检查路径
    /// </summary>
    public string GrpcHealthCheckPath { get; set; } = "/grpc/health";

    /// <summary>
    /// 心跳检测间隔(s)
    /// </summary>
    public int Interval { get; set; } = 3;

    /// <summary>
    /// 心跳超时时间(s)
    /// </summary>
    public int Timeout { get; set; } = 5;

    /// <summary>
    /// 心跳停止多久后注销服务(s)
    /// </summary>
    public int DeregisterTime { get; set; } = 10;

    /// <summary>
    /// 权重(使用权重调度器时有效)
    /// </summary>
    public int Weight { get; set; } = 1;
}