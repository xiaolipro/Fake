using Microsoft.AspNetCore.Http;

namespace Fake.AspNetCore.ExceptionHandling;

public interface IFakeHttpExceptionHandler
{
    Task<RemoteServiceErrorModel?> HandlerAndWarpErrorAsync(HttpContext httpContext, Exception exception);
}