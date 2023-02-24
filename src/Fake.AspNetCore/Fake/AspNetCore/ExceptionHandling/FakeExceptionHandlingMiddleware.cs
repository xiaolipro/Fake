using System;
using System.Threading.Tasks;
using Fake.DependencyInjection;
using Fake.ExceptionHandling;
using Fake.Identity.Authorization;
using Fake.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Fake.AspNetCore.ExceptionHandling;

/// <summary>
/// 异常处理中间件
/// </summary>
public class FakeExceptionHandlingMiddleware : IMiddleware, ITransientDependency
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
            // 如果响应已经开始了，中止该管道
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("一个异常发生了，但响应已启动！");
                throw;
            }

            if (context.Items["_FakeActionInfo"] is FakeMvcActionInfo { IsObjectResult: true })
            {
                await HandleAndWrapException(context, ex);
            }
        }
    }

    private async Task HandleAndWrapException(HttpContext httpContext, Exception exception)
    {
        _logger.LogException(exception);

        var serviceProvider = httpContext.RequestServices;
        await serviceProvider.GetRequiredService<IExceptionNotifier>()
            .NotifyAsync(new ExceptionNotificationContext(exception));

        if (exception is FakeAuthorizationException authorizationException)
        {
            await serviceProvider.GetRequiredService<IAuthorizationExceptionHandler>()
                .HandleAsync(authorizationException, httpContext);

            return;
        }

        var jsonSerializer = serviceProvider.GetRequiredService<IFakeJsonSerializer>();
        var statusCodeFinder = serviceProvider.GetRequiredService<IHttpExceptionStatusCodeFinder>();
        var converter = serviceProvider.GetRequiredService<IException2ErrorModelConverter>();
        var exceptionHandlingOptions = serviceProvider.GetRequiredService<IOptions<FakeExceptionHandlingOptions>>().Value;

        httpContext.Response.Clear();
        httpContext.Response.StatusCode = (int)statusCodeFinder.Find(httpContext, exception);
        httpContext.Response.OnStarting(ClearCacheHeaders, httpContext.Response);
        httpContext.Response.Headers.Add("Content-Type", "application/json");

        await httpContext.Response.WriteAsync(
            jsonSerializer.Serialize(
                converter.Convert(exception,
                    options => { options.OutputStackTrace = exceptionHandlingOptions.OutputStackTrace; }
                )
            )
        );
    }
}