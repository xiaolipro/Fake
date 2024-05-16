using System.Text;
using Fake.AspNetCore.Http;
using Fake.Authorization;
using Fake.ExceptionHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Fake.AspNetCore.ExceptionHandling;

public class FakeExceptionHandler(IStringLocalizer<FakeAspNetCoreResource> stringLocalizer) : IFakeExceptionHandler
{
    public virtual async Task<RemoteServiceErrorModel?> HandlerAndWarpErrorAsync(HttpContext httpContext,
        Exception exception)
    {
        await httpContext.RequestServices.GetRequiredService<IExceptionNotifier>()
            .NotifyAsync(new ExceptionNotificationContext(exception));

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

        return Convert(exception, exceptionOptions, httpContext.Response.StatusCode.ToString());
    }

    /// <summary>
    /// 转化异常
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="options"></param>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    protected virtual RemoteServiceErrorModel Convert(Exception exception, FakeExceptionHandlingOptions options,
        string statusCode)
    {
        var details = new StringBuilder();

        AddExceptionToDetails(exception, details, options.OutputStackTrace);

        var errorModel = new RemoteServiceErrorModel();

        return errorModel;
    }

    protected virtual void AddExceptionToDetails(Exception exception, StringBuilder details, bool outputStackTrace)
    {
        // message
        details.AppendLine(exception.GetType().Name + ": " + exception.Message);

        // stack trace
        if (outputStackTrace && !exception.StackTrace.IsNullOrEmpty())
        {
            details.AppendLine($"STRACK TRACE: {exception.StackTrace}");
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