using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Fake.AspNetCore.ExceptionHandling;

public interface IFakeHttpExceptionHandler
{
    Task<RemoteServiceErrorInfo?>
        HandlerAndWarpErrorAsync(ILogger logger, HttpContext httpContext, Exception exception);
}