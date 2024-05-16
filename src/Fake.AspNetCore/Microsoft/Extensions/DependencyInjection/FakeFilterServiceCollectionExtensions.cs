using Fake.AspNetCore.ExceptionHandling;
using Fake.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeFilterServiceCollectionExtensions
{
    public static IServiceCollection AddFakeExceptionFilter(this IServiceCollection services,
        Action<FakeExceptionHandlingOptions>? action = null)
    {
        services.AddFakeExceptionNotifier();

        services.TryAddTransient<FakeExceptionFilter>();
        services.TryAddTransient<IFakeExceptionHandler, FakeExceptionHandler>();
        services.TryAddTransient<IHttpExceptionStatusCodeFinder, DefaultHttpExceptionStatusCodeFinder>();

        services.Configure<MvcOptions>(options => { options.Filters.AddService<FakeExceptionFilter>(); });
        services.Configure<FakeExceptionHandlingOptions>(options =>
        {
            options.OutputStackTrace = services.GetInstanceOrNull<IWebHostEnvironment>()?.IsDevelopment() ?? false;
            action?.Invoke(options);
        });

        return services;
    }
}