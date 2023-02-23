using System;
using System.Threading.Tasks;
using Fake.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Fake.AspNetCore.ExceptionHandling;

public class FakeExceptionHandlingMiddleware:IMiddleware,ITransientDependency
{
    private readonly ILogger<FakeExceptionHandlingMiddleware> _logger;

    private static readonly Func<object, Task> ClearCacheHeaders = state =>
    {
        var response = (HttpResponse)state;

        response.Headers[HeaderNames.CacheControl] = "no-cache";
        response.Headers[HeaderNames.Pragma] = "no-cache";
        response.Headers[HeaderNames.Expires] = "-1";
        response.Headers.Remove(HeaderNames.ETag);

        return Task.CompletedTask;
    };

    public FakeExceptionHandlingMiddleware(ILogger<FakeExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // 如果响应已经开始了，中止程序
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("一个异常发生了，但响应已启动！");
                throw;
            }

            if (context.Items["_FakeActionInfo"] is FakeMvcActionInfo actionInfo)
            {
                if (actionInfo.IsObjectResult)
                {
                    await HandleAndWrapException(context, ex);
                }
            }
        }
    }

    private async Task HandleAndWrapException(HttpContext context, Exception exception)
    {
        _logger.log
    }
}