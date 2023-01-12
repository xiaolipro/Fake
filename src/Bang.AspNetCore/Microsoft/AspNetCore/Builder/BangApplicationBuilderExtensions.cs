using System.Diagnostics.CodeAnalysis;
using Bang;
using Bang.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder;

public static class BangApplicationBuilderExtensions
{
    public static void InitializeApplication([NotNull] this IApplicationBuilder app)
    {
        ThrowHelper.ThrowIfNull(app, nameof(app));

        app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;
        
        var application = app.ApplicationServices.GetRequiredService<BangApplication>();
        
        var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
        applicationLifetime.ApplicationStopping.Register(() => application.Shutdown());
        applicationLifetime.ApplicationStopped.Register(() => application.Dispose());

        application.InitializeModules(app.ApplicationServices);
    }
}