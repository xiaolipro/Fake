namespace Fake.AspNetCore.ExceptionHandling;

public static class FakeExceptionHandlingMiddlewareExtensions
{
    public const string FakeExceptionHandlingMiddlewareMarker = nameof(FakeExceptionHandlingMiddlewareMarker);

    public static IApplicationBuilder UseFakeExceptionHandling(this IApplicationBuilder app)
    {
        return app.VerifyMiddlewareAreRegistered(FakeExceptionHandlingMiddlewareMarker)
            ? app
            : app.UseMiddleware<FakeExceptionHandlingMiddleware>();
    }
}