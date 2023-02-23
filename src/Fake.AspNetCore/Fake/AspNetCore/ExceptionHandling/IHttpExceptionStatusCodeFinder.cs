using System;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Fake.AspNetCore.ExceptionHandling;

public interface IHttpExceptionStatusCodeFinder
{
    HttpStatusCode Find(HttpContext httpContext, Exception exception);
}