using Fake.AspNetCore.ExceptionHandling;
using Fake.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeFilterServiceCollectionExtensions
{
    public static IServiceCollection AddFilter<TFilter>(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient) where TFilter : class, IFilterMetadata
    {
        services.TryAdd(new ServiceDescriptor(typeof(TFilter), typeof(TFilter), lifetime));
        services.Configure<MvcOptions>(options => { options.Filters.AddService<TFilter>(); });
        return services;
    }

    public static IServiceCollection AddFakeExceptionFilter(this IServiceCollection services,
        Action<FakeExceptionHandlingOptions>? action = null)
    {
        services.AddFakeExceptionNotifier();

        services.TryAddTransient<IFakeHttpExceptionHandler, FakeHttpExceptionHandler>();
        services.TryAddTransient<IHttpExceptionStatusCodeFinder, DefaultHttpExceptionStatusCodeFinder>();

        services.AddFilter<FakeExceptionFilter>();
        services.Configure<FakeExceptionHandlingOptions>(options =>
        {
            options.OutputStackTrace = services.GetInstanceOrNull<IWebHostEnvironment>()?.IsDevelopment() ?? false;
            action?.Invoke(options);
        });

        return services;
    }

    public static IServiceCollection AddFakeValidationActionFilter(this IServiceCollection services)
    {
        return services.AddFilter<FakeValidationActionFilter>();
    }
}