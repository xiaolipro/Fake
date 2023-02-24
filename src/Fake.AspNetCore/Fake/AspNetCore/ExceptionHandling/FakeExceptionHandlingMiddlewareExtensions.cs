using Microsoft.AspNetCore.Builder;

namespace Fake.AspNetCore.ExceptionHandling;

public static class FakeExceptionHandlingMiddlewareExtensions
{
    public const string FakeExceptionHandlingMiddlewareMarker = nameof(FakeExceptionHandlingMiddlewareMarker);

    public static IApplicationBuilder UseAbpExceptionHandling(this IApplicationBuilder app)
    {
        // 防呆
        if (app.Properties.ContainsKey(FakeExceptionHandlingMiddlewareMarker))
        {
            return app;
        }

        app.Properties[FakeExceptionHandlingMiddlewareMarker] = true;
        return app.UseMiddleware<FakeExceptionHandlingMiddleware>();
    }
}