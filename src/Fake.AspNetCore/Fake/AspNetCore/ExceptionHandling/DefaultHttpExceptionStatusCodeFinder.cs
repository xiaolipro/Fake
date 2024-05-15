using System.Net;
using Fake.Authorization;
using Fake.ExceptionHandling;
using Microsoft.AspNetCore.Http;

namespace Fake.AspNetCore.ExceptionHandling;

public class DefaultHttpExceptionStatusCodeFinder(IOptions<FakeHttpExceptionStatusOptions> options)
    : IHttpExceptionStatusCodeFinder
{
    private readonly FakeHttpExceptionStatusOptions _options = options.Value;

    public virtual HttpStatusCode Find(HttpContext httpContext, Exception exception)
    {
        if (exception is IHasErrorCode exceptionWithErrorCode)
        {
            var code = exceptionWithErrorCode.Code;
            if (!code.IsNullOrWhiteSpace())
            {
                if (_options.ErrorCodeToHttpStatusCodeMappings.TryGetValue(code!, out var statusCode))
                {
                    return statusCode;
                }
            }
        }

        if (exception is FakeAuthorizationException)
        {
            var isAuthenticated = httpContext.User.Identity?.IsAuthenticated ?? false;
            return isAuthenticated
                ? HttpStatusCode.Forbidden
                : HttpStatusCode.Unauthorized;
        }

        if (exception is NotImplementedException) return HttpStatusCode.NotImplemented;

        if (exception is BusinessException) return HttpStatusCode.BadRequest;

        return HttpStatusCode.InternalServerError;
    }
}