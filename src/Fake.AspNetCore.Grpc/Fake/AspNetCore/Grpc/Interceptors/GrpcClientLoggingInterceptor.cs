using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace Fake.AspNetCore.Grpc.Interceptors;

/// <summary>
/// Grpc客户端日志拦截器
/// </summary>
/// <remarks>
/// <para>Description see: https://docs.microsoft.com/zh-cn/aspnet/core/grpc/interceptors?view=aspnetcore-6.0</para>
/// <para>Impl see: https://github.com/grpc/grpc-dotnet/blob/master/examples/Interceptor/Server/ServerLoggerInterceptor.cs</para>
/// </remarks>
public class GrpcClientLoggingInterceptor : Interceptor
{
    private readonly ILogger<GrpcClientLoggingInterceptor> _logger;

    public GrpcClientLoggingInterceptor(ILogger<GrpcClientLoggingInterceptor> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 截获一元 RPC 的阻塞调用。
    /// </summary>
    /// <remarks>
    /// Tips: 尽管 BlockingUnaryCall 和 AsyncUnaryCall 都是指一元 RPC，但二者不可互换。 
    /// 阻塞调用不会被 AsyncUnaryCall 截获，异步调用不会被 BlockingUnaryCall 截获。
    /// </remarks>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <returns></returns>
    public override TResponse BlockingUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        WriteLog(context);
        AddCallerMetadata(ref context);

        try
        {
            // => return continuation(request, context);
            return base.BlockingUnaryCall(request, context, continuation);
        }
        catch (Exception e)
        {
            throw new FakeGrpcException(e.Message, e.InnerException);
        }
    }


    /// <summary>
    /// 截获一元 RPC 的异步调用。
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <returns></returns>
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        WriteLog(context);
        AddCallerMetadata(ref context);

        try
        {
            var call = continuation(request, context);

            return new AsyncUnaryCall<TResponse>(HandleResponse(call.ResponseAsync), call.ResponseHeadersAsync,
                call.GetStatus, call.GetTrailers, call.Dispose);
        }
        catch (Exception e)
        {
            throw new FakeGrpcException(e.Message);
        }
    }

    protected virtual async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> t)
    {
        try
        {
            var response = await t;
            _logger.LogInformation("Response received: {Response}", response);
            return response;
        }
        catch (Exception ex)
        {
            throw new FakeGrpcException(ex.Message);
        }
    }


    /// <summary>
    /// 截获客户端流式处理 RPC 的异步调用。
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <returns></returns>
    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        WriteLog(context);
        AddCallerMetadata(ref context);

        try
        {
            return base.AsyncClientStreamingCall(context, continuation);
        }
        catch (Exception e)
        {
            throw new FakeGrpcException(e.Message);
        }
    }

    /// <summary>
    /// 截获服务器流式处理 RPC 的异步调用。
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <returns></returns>
    public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
        TRequest request, ClientInterceptorContext<TRequest, TResponse> context,
        AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        WriteLog(context);
        AddCallerMetadata(ref context);

        try
        {
            return base.AsyncServerStreamingCall(request, context, continuation);
        }
        catch (Exception e)
        {
            throw new FakeGrpcException(e.Message);
        }
    }

    /// <summary>
    /// 截获双向流式处理 RPC 的异步调用。
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <returns></returns>
    public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        WriteLog(context);
        AddCallerMetadata(ref context);

        try
        {
            return continuation(context);
        }
        catch (Exception e)
        {
            throw new FakeGrpcException(e.Message);
        }
    }


    /// <summary>
    /// 添加调用者元数据到请求头
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="context"></param>
    protected virtual void AddCallerMetadata<TRequest, TResponse>(ref ClientInterceptorContext<TRequest, TResponse> context)
        where TRequest : class
        where TResponse : class
    {
        var headers = context.Options.Headers;


        // 如果当前上下文没有headers，创建带有headers的新上下文
        if (headers is null)
        {
            headers = new Metadata();
            var options = context.Options.WithHeaders(headers);

            context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);
        }

        // 添加调用者metadata到请求头
        headers.Add("caller-user", Environment.UserName);
        headers.Add("caller-machine", Environment.MachineName);
        headers.Add("caller-os", Environment.OSVersion.ToString());
    }

    /// <summary>
    /// 记录日志
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="context"></param>
    protected virtual void WriteLog<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context)
        where TRequest : class
        where TResponse : class
    {
        _logger.LogInformation(
            "Grpc客户端开始调用，主机{Host}，类型：{MethodType}，方法：{MethodName}，请求模型：{TRequest}，响应模型：{TResponse}",
            context.Host, context.Method.Type, context.Method.Name, typeof(TRequest), typeof(TResponse));
    }
}