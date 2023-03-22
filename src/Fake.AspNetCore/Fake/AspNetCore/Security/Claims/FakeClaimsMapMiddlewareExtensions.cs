using Microsoft.AspNetCore.Builder;

namespace Fake.AspNetCore.Security.Claims;

public static class FakeClaimsMapMiddlewareExtensions
{
    public const string FakeClaimsMapMiddlewareMaker = nameof(FakeClaimsMapMiddlewareMaker);
    
    public static IApplicationBuilder UseFakeClaimsMap(this IApplicationBuilder app)
    {
        return app.VerifyMiddlewareAreRegistered(FakeClaimsMapMiddlewareMaker)
            ? app
            : app.UseMiddleware<FakeClaimsMapMiddleware>();
    }
}