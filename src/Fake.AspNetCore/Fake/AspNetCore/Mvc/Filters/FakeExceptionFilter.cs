using System.Text;
using Fake.AspNetCore.ExceptionHandling;
using Fake.Authorization;
using Fake.ExceptionHandling;
using Fake.Json;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Fake.AspNetCore.Mvc.Filters;

public class FakeExceptionFilter(ILogger<FakeExceptionFilter> logger) : IAsyncExceptionFilter
{
    public virtual async Task OnExceptionAsync(ExceptionContext context)
    {
        if (!ShouldHandle(context)) return;

        await HandleAndWrapExceptionAsync(context);
    }

    private bool ShouldHandle(ExceptionContext context)
    {
        if (context.ExceptionHandled) return false;

        if (context.ActionDescriptor is ControllerActionDescriptor)
        {
            return true;
        }

        return false;
    }

    protected virtual async Task HandleAndWrapExceptionAsync(ExceptionContext context)
    {
        //TODO: Trigger an AbpExceptionHandled event or something like that.

        LogException(context, out var remoteServiceErrorInfo);

        var httpContext = context.HttpContext;
        await httpContext.RequestServices.GetRequiredService<IExceptionNotifier>()
            .NotifyAsync(new ExceptionNotificationContext(context.Exception, httpContext.RequestServices));

        if (context.Exception is FakeAuthorizationException authorizationException)
        {
            await httpContext.RequestServices.GetRequiredService<IAuthorizationExceptionHandler>()
                .HandleAsync(authorizationException, httpContext);
        }
        else
        {
            var statusCodeFinder = httpContext.RequestServices.GetRequiredService<IHttpExceptionStatusCodeFinder>();
            context.HttpContext.Response.StatusCode = (int)statusCodeFinder.Find(httpContext, context.Exception);

            context.Result = new ObjectResult(remoteServiceErrorInfo);
        }

        context.ExceptionHandled = true; // Handled!
    }

    protected virtual void LogException(ExceptionContext context, out RemoteServiceErrorInfo remoteServiceErrorInfo)
    {
        var exceptionHandlingOptions = context.HttpContext.RequestServices
            .GetRequiredService<IOptions<FakeExceptionHandlingOptions>>()
            .Value;
        var exceptionToErrorInfoConverter = context.HttpContext.RequestServices
            .GetRequiredService<IExceptionToErrorInfoConverter>();
        remoteServiceErrorInfo = exceptionToErrorInfoConverter.Convert(context.Exception, exceptionHandlingOptions);

        var remoteServiceErrorInfoBuilder = new StringBuilder();
        remoteServiceErrorInfoBuilder.AppendLine($"---------- {nameof(RemoteServiceErrorInfo)} ----------");
        remoteServiceErrorInfoBuilder.AppendLine(context.HttpContext.RequestServices
            .GetRequiredService<IFakeJsonSerializer>()
            .Serialize(remoteServiceErrorInfo, indented: true));

        var logLevel = context.Exception.GetLogLevel();
        logger.LogWithLevel(logLevel, remoteServiceErrorInfoBuilder.ToString());
        logger.LogException(context.Exception, logLevel);
    }
}