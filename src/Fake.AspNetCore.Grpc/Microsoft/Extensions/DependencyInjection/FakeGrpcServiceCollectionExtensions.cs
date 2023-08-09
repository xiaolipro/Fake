using System;
using System.Net.Http;
using System.Threading;
using Fake.AspNetCore.Grpc.Balancer;
using Fake.AspNetCore.Grpc.Interceptors;
using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SkyApm.Diagnostics.Grpc.Client;
using SkyApm.Diagnostics.Grpc.Server;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeGrpcServiceCollectionExtensions
{
    /// <summary>
    /// 添加Grpc负载均衡客户端
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <param name="services"></param>
    /// <param name="address"></param>
    /// <param name="toApm"></param>
    public static IServiceCollection AddGrpcLoadBalancingClient<TClient>(
        this IServiceCollection services, string address, bool toApm = true)
        where TClient : class
    {
        services.TryAddSingleton<IBackoffPolicyFactory, FakeBackoffPolicyFactory>();
        services.TryAddSingleton<ResolverFactory, FakeResolverFactory>();
        services.TryAddSingleton<LoadBalancerFactory, FakeBalancerFactory>();
        
        var serviceConfig = new ServiceConfig()
        {
            // 自定义负载均衡
            LoadBalancingConfigs = { new LoadBalancingConfig(nameof(FakeBalancerFactory)) },
            MethodConfigs =
            {
                new MethodConfig
                {
                    // MethodName.Default 将应用于此通道调用的所有 gRPC 方法
                    Names = { MethodName.Default },

                    /* 重试策略
                     * 满足以下所有条件时，将重试调用：
                     * 1. 失败状态代码与 RetryableStatusCodes 中的值匹配。
                     * 2. 之前的尝试次数小于 MaxAttempts。
                     * 3. 此调用 未提交。
                     * 4. 尚未超过截止时间。
                     * 在以下两种情况下，将提交 gRPC 调用：
                     * 1. 客户端收到响应头。 调用 ServerCallContext.WriteResponseHeadersAsync 或将第一个消息写入服务器响应流时，服务器会发送响应头。
                     * 2. 客户端的传出消息（如果是流式处理则为消息）已超出客户端的最大缓冲区大小。 MaxRetryBufferSize 和 MaxRetryBufferPerCallSize在通道上配置。
                     * !. 无论状态代码是什么或以前的尝试次数是多少，提交的调用都无法重试。
                     * details see: https://docs.microsoft.com/zh-cn/aspnet/core/grpc/retries?view=aspnetcore-6.0
                     */
                    RetryPolicy = new RetryPolicy
                    {
                        MaxAttempts = 5,
                        InitialBackoff = TimeSpan.FromSeconds(1),
                        MaxBackoff = TimeSpan.FromSeconds(5),
                        BackoffMultiplier = 1.5,
                        RetryableStatusCodes = { StatusCode.Unavailable, StatusCode.Internal }
                    }
                }
            }
        };
        var httpClientBuilder = services
            .AddGrpcClient<TClient>(options =>
            {
                options.Address = new Uri($"{nameof(FakeResolverFactory)}://" + address);
            })
            .ConfigureChannel(options =>
            {
                options.Credentials = ChannelCredentials.Insecure;
                options.ServiceConfig = serviceConfig;
                //options.ServiceProvider = services.BuildServiceProvider();

                /* 连接持活
                 在非活动期间每 60 秒向服务器发送一次保持活动 ping, 确保服务器和使用中的任何代理不会由于不活动而关闭连接。
                 details see: https://docs.microsoft.com/zh-cn/aspnet/core/grpc/performance?view=aspnetcore-6.0
                 */
                options.HttpHandler = new SocketsHttpHandler
                {
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                    // 每隔60s向服务器发送一次ping，确保服务器和使用中的任何代理不会由于不活动而关闭连接
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                    EnableMultipleHttp2Connections = true
                };
            })
            .AddInterceptor<GrpcClientLoggingInterceptor>();

        // notes: 客户端可能没有管道流程
        if (toApm) httpClientBuilder.AddInterceptor<ClientDiagnosticInterceptor>();

        return services;
    }


    /// <summary>
    /// 添加Grpc服务端
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddGrpcServer(this IServiceCollection services)
    {
        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<GrpcServerLoggingInterceptor>();
            options.Interceptors.Add<ServerDiagnosticInterceptor>();
        });

        return services;
    }
}