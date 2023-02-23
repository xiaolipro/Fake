using System.Threading.Tasks;
using Fake.Identity.Authorization;
using Microsoft.AspNetCore.Http;

namespace Fake.AspNetCore.ExceptionHandling;

public interface IFakeAuthorizationExceptionHandler
{
    Task HandleAsync(FakeAuthorizationException exception, HttpContext httpContext);
}