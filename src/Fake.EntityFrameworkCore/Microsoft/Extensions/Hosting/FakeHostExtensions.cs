using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting;

public static class FakeHostExtensions
{
    /// <summary>
    /// 迁移数据库上下文
    /// </summary>
    /// <param name="host"></param>
    /// <param name="seeder">种子行为</param>
    /// <typeparam name="TContext">数据库上下文</typeparam>
    /// <returns></returns>
    public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
        where TContext : DbContext
    {
        ThrowHelper.ThrowIfNull(seeder);
        var underK8S = host.IsInKubernetes();

        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetRequiredService<TContext>();

        try
        {
            logger.LogInformation("Migrating database associated with context {DbContextName}",
                typeof(TContext).Name);

            // todo: 网络回退？
            InvokeSeeder(seeder!, context, services);

            logger.LogInformation("Migrated database associated with context {DbContextName}",
                typeof(TContext).Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}",
                typeof(TContext).Name);
            if (underK8S)
            {
                throw; // Rethrow under k8s because we rely on k8s to re-run the pod
            }
        }

        return host;
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context,
        IServiceProvider services)
        where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
}