using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fake.AspNetCore.SwaggerGen;

public class SwaggerConfigureOptions(IApiDescriptionGroupCollectionProvider provider)
    : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiDescriptionGroups.Items)
        {
            options.SwaggerDoc(description.GroupName ?? "default",
                new OpenApiInfo { Title = description.GroupName, Version = "v1" });
        }
    }
}