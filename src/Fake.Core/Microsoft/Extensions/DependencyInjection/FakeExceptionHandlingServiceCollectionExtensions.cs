using Fake.ExceptionHandling;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeExceptionHandlingServiceCollectionExtensions
{
    public static IServiceCollection AddFakeExceptionNotifier(this IServiceCollection services)
    {
        services.TryAddTransient<IExceptionNotifier, ExceptionNotifier>();

        return services;
    }


    public static IServiceCollection AddNullExceptionNotifier(this IServiceCollection services)
    {
        services.TryAddTransient<IExceptionNotifier, NullExceptionNotifier>();

        return services;
    }
}