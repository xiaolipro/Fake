using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace Fake.AspNetCore.Grpc.Interceptors;

/// <summary>
/// Grpc服务端日志拦截器
/// </summary>
/// <remarks>
/// <para>Description see: https://docs.microsoft.com/zh-cn/aspnet/core/grpc/interceptors?view=aspnetcore-6.0</para>
/// <para>Impl see: https://github.com/grpc/grpc-dotnet/blob/master/examples/Interceptor/Server/ServerLoggerInterceptor.cs</para>
/// </remarks>
public class GrpcServerLoggingInterceptor : Interceptor
{
    private readonly ILogger<GrpcServerLoggingInterceptor> _logger;

    public GrpcServerLoggingInterceptor(ILogger<GrpcServerLoggingInterceptor> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 截获一元 RPC。
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <returns></returns>
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        WriteLog<TRequest, TResponse>(MethodType.Unary, context);
        try
        {
            return await base.UnaryServerHandler(request, context, continuation);
        }
        catch (Exception e)
        {
            throw new FakeGrpcException(e.Message);
        }
    }

    /// <summary>
    /// 截获客户端流式处理 RPC
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="requestStream"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <returns></returns>
    public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        WriteLog<TRequest, TResponse>(MethodType.ClientStreaming, context);
        try
        {
            return base.ClientStreamingServerHandler(requestStream, context, continuation);
        }
        catch (Exception e)
        {
            throw new FakeGrpcException(e.Message);
        }
    }

    /// <summary>
    /// 截获服务器流式处理 RPC。
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="responseStream"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <returns></returns>
    public override Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        WriteLog<TRequest, TResponse>(MethodType.ServerStreaming, context);
        try
        {
            return base.ServerStreamingServerHandler(request, responseStream, context, continuation);
        }
        catch (Exception e)
        {
            throw new FakeGrpcException(e.Message);
        }
    }

    /// <summary>
    /// 截获双向流式处理 RPC。
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="requestStream"></param>
    /// <param name="responseStream"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <returns></returns>
    public override Task DuplexStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        WriteLog<TRequest, TResponse>(MethodType.DuplexStreaming, context);
        try
        {
            return base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
        }
        catch (Exception e)
        {
            throw new FakeGrpcException(e.Message);
        }
    }


    /// <summary>
    /// 写日志
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="methodType"></param>
    /// <param name="context"></param>
    private void WriteLog<TRequest, TResponse>(MethodType methodType, ServerCallContext context)
        where TRequest : class
        where TResponse : class
    {
        _logger.LogInformation("Grpc服务端开始处理，类型：{MethodType}， 请求类型：{TRequest}，响应类型：{TResponse}", methodType,
            typeof(TRequest), typeof(TResponse));
        WriteMetadata(context.RequestHeaders, "caller-user");
        WriteMetadata(context.RequestHeaders, "caller-machine");
        WriteMetadata(context.RequestHeaders, "caller-os");

        void WriteMetadata(Metadata headers, string key)
        {
            var headerValue = headers.GetValue(key) ?? "(unknown)";
            _logger.LogInformation("{Key}: {HeaderValue}", key, headerValue);
        }
    }
}