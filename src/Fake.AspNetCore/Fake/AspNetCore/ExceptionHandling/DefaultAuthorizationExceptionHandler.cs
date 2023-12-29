using System;
using System.Threading.Tasks;
using Fake.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fake.AspNetCore.ExceptionHandling;

public class DefaultAuthorizationExceptionHandler : IAuthorizationExceptionHandler
{
    public async Task HandleAsync(FakeAuthorizationException exception, HttpContext httpContext)
    {
        var serviceProvider = httpContext.RequestServices;
        var handlerOptions = serviceProvider.GetRequiredService<IOptions<FakeAuthorizationExceptionHandlerOptions>>()
            .Value;
        var authenticationSchemeProvider = serviceProvider.GetRequiredService<IAuthenticationSchemeProvider>();

        AuthenticationScheme? scheme;
        var isAuthenticated = httpContext.User.Identity?.IsAuthenticated ?? false;

        if (!handlerOptions.AuthenticationScheme.IsNullOrWhiteSpace())
        {
            scheme = await authenticationSchemeProvider.GetSchemeAsync(handlerOptions.AuthenticationScheme!);

            if (scheme is null)
            {
                throw new FakeException($"找不到AuthenticationScheme：{handlerOptions.AuthenticationScheme}.");
            }
        }
        else
        {
            if (isAuthenticated)
            {
                scheme = await authenticationSchemeProvider.GetDefaultForbidSchemeAsync();
                if (scheme is null)
                {
                    throw new FakeException("找不到DefaultForbidScheme.");
                }
            }
            else
            {
                scheme = await authenticationSchemeProvider.GetDefaultChallengeSchemeAsync();
                if (scheme is null)
                {
                    throw new FakeException("找不到DefaultChallengeScheme.");
                }
            }
        }

        var handlers = serviceProvider.GetRequiredService<IAuthenticationHandlerProvider>();
        var handler = await handlers.GetHandlerAsync(httpContext, scheme.Name);

        if (handler is null)
        {
            throw new FakeException($"找不到{scheme.Name}的handler.");
        }

        if (isAuthenticated)
        {
            await handler.ForbidAsync(null);
        }
        else
        {
            await handler.ChallengeAsync(null);
        }
    }
}