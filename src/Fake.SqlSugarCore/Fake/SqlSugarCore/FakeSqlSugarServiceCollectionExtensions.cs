using Fake.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.SqlSugarCore;

public static class FakeSqlSugarServiceCollectionExtensions
{
    public static IServiceCollection AddSugarDbContext<T>(this IServiceCollection services,
        Action<SugarDbConnOptions>? action = null) where T : SugarDbContext
    {
        var configuration = services.GetConfiguration();
        var options = configuration.GetSection(typeof(T).Name).Get<SugarDbConnOptions>() ?? new();

        services.AddScoped<SugarDbContext, T>(provider =>
        {
            action?.Invoke(options);
            return ReflectionHelper.CreateInstance<T>(typeof(T), options);
        });

        return services;
    }
}