using Fake.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Fake.AspNetCore.ExceptionHandling;

/// <summary>
/// 异常处理中间件
/// </summary>
public class FakeExceptionHandlingMiddleware(
    ILogger<FakeExceptionHandlingMiddleware> logger,
    IFakeExceptionHandler converter) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // 如果响应已经开始了，中止该管道
            if (context.Response.HasStarted)
            {
                logger.LogWarning("一个异常发生了，但响应已经开始了！");
                throw;
            }

            var errorModel = await converter.HandlerAndWarpErrorAsync(context, ex);
            if (errorModel == null) return;
            var jsonSerializer = context.RequestServices.GetRequiredService<IFakeJsonSerializer>();

            context.Response.Clear();
            context.Response.Headers.Append("Content-Type", "application/json");
            context.Response.OnStarting(ClearCacheHeaders, context.Response);
            await context.Response.WriteAsync(jsonSerializer.Serialize(errorModel));
        }
    }

    static readonly Func<object, Task> ClearCacheHeaders = state =>
    {
        var response = (HttpResponse)state;

        response.Headers[HeaderNames.CacheControl] = "no-cache";
        response.Headers[HeaderNames.Pragma] = "no-cache";
        response.Headers[HeaderNames.Expires] = "-1";
        response.Headers.Remove(HeaderNames.ETag);

        return Task.CompletedTask;
    };
}