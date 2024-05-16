using Fake.AspNetCore.Http;
using Microsoft.AspNetCore.Http;

namespace Fake.AspNetCore.ExceptionHandling;

public interface IFakeExceptionHandler
{
    Task<RemoteServiceErrorModel> HandlerAndWarpErrorAsync(HttpContext httpContext, Exception exception);
}