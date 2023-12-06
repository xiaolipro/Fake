using System;
using Fake.AspNetCore.Grpc.Balancer;
using Grpc.Net.Client.Configuration;

namespace Fake.AspNetCore.Grpc;

public class FakeGrpcClientOptions
{
    /// <summary>
    /// grpc client默认提供两种解析：dns和static，当然，你也可以通过实现<see cref="FakeGrpcResolverFactory"/>自定义
    /// </summary>
    public string? Resolver { get; set; }

    /// <summary>
    /// grpc client默认提供两种负载均衡策略：pick_first和round_robin，当然，你也可以通过实现<see cref="FakeGrpcBalancerFactory"/>自定义
    /// </summary>
    public string? Balancer { get; set; }

    /// <summary>
    /// 服务地址缓存刷新间隔（适用于dns解析、自定义解析）
    /// 在使用负载均衡时，性能非常重要。 通过缓存地址，从 gRPC 调用中消除了解析地址的延迟。进行第一次 gRPC 调用时，将调用解析程序，后续调用将使用缓存。
    /// 如果连接中断，则会自动刷新地址。 如果是在运行时更改地址，那么刷新非常重要。
    /// 例如，在 Kubernetes 中，重启的 Pod 会触发 DNS 解析程序刷新并获取 Pod 的新地址。
    /// 默认情况下，如果连接中断，则会刷新 DNS 解析程序。 DNS 解析程序还可以根据需要定期刷新。 这对于快速检测新的 pod 实例很有用。
    /// </summary>
    public TimeSpan RefreshInterval { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// 客户端解析不到服务，会根据回退策略提供的时间，不断重试。
    /// </summary>
    public TimeSpan BackoffInterval { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// 重试策略
    /// </summary>
    public RetryPolicy? RetryPolicy { get; set; }
}