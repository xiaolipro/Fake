namespace Fake.AspNetCore.Security;

public static class FakeSecurityHeadersMiddlewareExtensions
{
    public static IApplicationBuilder UseFakeSecurityHeaders(this IApplicationBuilder app)
    {
        return app.UseMiddleware<FakeSecurityHeadersMiddleware>();
    }
}