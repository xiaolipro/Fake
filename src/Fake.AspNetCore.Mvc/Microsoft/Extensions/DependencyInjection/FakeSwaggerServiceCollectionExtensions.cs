using Fake.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeSwaggerServiceCollectionExtensions
{
    public static IServiceCollection AddFakeSwaggerGen(this IServiceCollection services,
        Action<SwaggerGenOptions>? action = null)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();

        services.AddSwaggerGen(options =>
        {
            foreach (var item in Directory.GetFiles(AppContext.BaseDirectory, "*.xml"))
            {
                options.IncludeXmlComments(item, true);
            }
        });

        return services;
    }
}