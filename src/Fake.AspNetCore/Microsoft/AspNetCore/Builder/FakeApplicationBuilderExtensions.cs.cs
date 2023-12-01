using Fake;
using Fake.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder;

public static class FakeApplicationBuilderExtensions
{
    public static void InitializeApplication(this IApplicationBuilder app)
    {
        ThrowHelper.ThrowIfNull(app, nameof(app));

        app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;

        var application = app.ApplicationServices.GetRequiredService<FakeApplication>();

        var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
        applicationLifetime.ApplicationStopped.Register(() => application.Dispose());

        application.InitializeApplication(app.ApplicationServices);
    }

    /// <summary>
    /// 防呆设计，验证中间件是否已经注册
    /// </summary>
    /// <param name="app"></param>
    /// <param name="marker"></param>
    /// <returns></returns>
    public static bool VerifyMiddlewareAreRegistered(this IApplicationBuilder app, string marker)
    {
        ThrowHelper.ThrowIfNull(app, nameof(app));

        if (app.Properties.ContainsKey(marker))
        {
            return true;
        }

        app.Properties[marker] = true;
        return false;
    }
}