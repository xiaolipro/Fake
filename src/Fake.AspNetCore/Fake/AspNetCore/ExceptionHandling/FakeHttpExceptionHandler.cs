using System.Text;
using Fake.AspNetCore.Localization;
using Fake.AspNetCore.Mvc.Filters;
using Fake.Authorization;
using Fake.ExceptionHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fake.AspNetCore.ExceptionHandling;

public class FakeHttpExceptionHandler(IStringLocalizer<FakeAspNetCoreResource> localizer) : IFakeHttpExceptionHandler
{
    public virtual async Task<RemoteServiceErrorModel?> HandlerAndWarpErrorAsync(HttpContext httpContext,
        Exception exception)
    {
        await httpContext.RequestServices.GetRequiredService<IExceptionNotifier>()
            .NotifyAsync(new ExceptionNotificationContext(exception, httpContext.RequestServices));

        if (exception is FakeAuthorizationException authorizationException)
        {
            await httpContext.RequestServices.GetRequiredService<IAuthorizationExceptionHandler>()
                .HandleAsync(authorizationException, httpContext);
            return default;
        }

        var statusCodeFinder = httpContext.RequestServices.GetRequiredService<IHttpExceptionStatusCodeFinder>();
        var exceptionOptions = httpContext.RequestServices.GetRequiredService<IOptions<FakeExceptionHandlingOptions>>()
            .Value;

        httpContext.Response.StatusCode = (int)statusCodeFinder.Find(httpContext, exception);

        var errorModel = Convert(exception, exceptionOptions);

        var logger = httpContext.RequestServices.GetService<ILogger<FakeExceptionFilter>>() ??
                     NullLogger<FakeExceptionFilter>.Instance;

        var remoteServiceErrorLogBuilder = new StringBuilder();
        remoteServiceErrorLogBuilder.AppendLine("|- Remote service call error occurred");
        remoteServiceErrorLogBuilder.AppendLine($"|- TraceIdentifier: {httpContext.TraceIdentifier}");
        remoteServiceErrorLogBuilder.AppendLine(
            $"|- RequestPath    : {httpContext.Request.Path}{httpContext.Request.QueryString}");
        remoteServiceErrorLogBuilder.AppendLine($"|- ErrorType      : {errorModel.Type}");
        remoteServiceErrorLogBuilder.AppendLine($"|- ErrorDetails   : {errorModel.Message}");
        logger.LogWithLevel(exception.GetLogLevel(), remoteServiceErrorLogBuilder.ToString());

        return errorModel;
    }

    /// <summary>
    /// 转化异常
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    protected virtual RemoteServiceErrorModel Convert(Exception exception, FakeExceptionHandlingOptions options)
    {
        var errorModel = new RemoteServiceErrorModel
        {
            Type = localizer[exception.GetType().Name]
        };

        var details = new StringBuilder();
        AddExceptionToDetails(exception, details, options.OutputStackTrace);
        errorModel.Message = details.ToString();


        return errorModel;
    }

    protected virtual void AddExceptionToDetails(Exception exception, StringBuilder details, bool outputStackTrace)
    {
        // message
        details.AppendLine(exception.GetType().Name + ": " + exception.Message);

        // stack trace
        if (outputStackTrace && !exception.StackTrace.IsNullOrEmpty())
        {
            details.AppendLine($"Stack Trace: {exception.StackTrace}");
        }

        // inner exception
        if (exception.InnerException != null)
        {
            AddExceptionToDetails(exception.InnerException, details, outputStackTrace);
        }

        // aggregate exception
        if (exception is AggregateException aggregateException)
        {
            foreach (var innerException in aggregateException.InnerExceptions)
            {
                AddExceptionToDetails(innerException, details, outputStackTrace);
            }
        }
    }
}