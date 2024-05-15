namespace Microsoft.AspNetCore.Builder;

public static class FakeSwaggerApplicationBuilderExtensions
{
    public static IApplicationBuilder UseFakeSwagger(this IApplicationBuilder app)
    {
        //启用swagger中间件
        app.UseSwagger();

        // 启用swagger-ui中间件，指定swagger json文件路径
        app.UseSwaggerUI(options =>
        {
            var apiDescriptionGroups = app.ApplicationServices
                .GetRequiredService<IApiDescriptionGroupCollectionProvider>().ApiDescriptionGroups.Items;

            foreach (var description in apiDescriptionGroups)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
            }
        });

        return app;
    }
}