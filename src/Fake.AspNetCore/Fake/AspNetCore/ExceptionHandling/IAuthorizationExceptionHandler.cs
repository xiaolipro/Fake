using Fake.Authorization;
using Microsoft.AspNetCore.Http;

namespace Fake.AspNetCore.ExceptionHandling;

public interface IAuthorizationExceptionHandler
{
    Task HandleAsync(FakeAuthorizationException exception, HttpContext httpContext);
}