﻿using System.Diagnostics.CodeAnalysis;
using Fake;
using Fake.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder;

public static class FakeApplicationBuilderExtensions
{
    public static void InitializeApplication([NotNull] this IApplicationBuilder app)
    {
        ThrowHelper.ThrowIfNull(app, nameof(app));

        app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;
        
        var application = app.ApplicationServices.GetRequiredService<FakeApplication>();
        
        var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
        applicationLifetime.ApplicationStopping.Register(() => application.Shutdown());
        applicationLifetime.ApplicationStopped.Register(() => application.Dispose());

        application.Initialize(app.ApplicationServices);
    }
}