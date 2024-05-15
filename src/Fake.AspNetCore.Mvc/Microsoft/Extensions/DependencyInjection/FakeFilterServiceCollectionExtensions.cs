using Fake.AspNetCore.Mvc.Filters.UnifiedResult;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeFilterServiceCollectionExtensions
{
    public static IServiceCollection AddUnifiedResultFilter(this IServiceCollection services)
    {
        services.AddSingleton<UnifiedResultFilter>();
        services.AddSingleton<IUnifiedResultHandler, DefaultUnifiedResultHandler>();
        services.Configure<MvcOptions>(options => { options.Filters.Add<UnifiedResultFilter>(); });
        return services;
    }
}