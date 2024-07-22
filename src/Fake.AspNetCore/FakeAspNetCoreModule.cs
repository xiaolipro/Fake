using Fake.AspNetCore.ExceptionHandling;
using Fake.AspNetCore.Http;
using Fake.AspNetCore.Localization;
using Fake.AspNetCore.Mvc.ApiExplorer;
using Fake.AspNetCore.Mvc.Conventions;
using Fake.AspNetCore.Security.Claims;
using Fake.AspNetCore.VirtualFileSystem;
using Fake.Authorization;
using Fake.DomainDrivenDesign;
using Fake.Localization;
using Fake.Modularity;
using Fake.Security.Claims;
using Fake.VirtualFileSystem;

// ReSharper disable once CheckNamespace
namespace Fake.AspNetCore;

[DependsOn(
    typeof(FakeVirtualFileSystemModule),
    typeof(FakeAuthorizationModule),
    typeof(FakeDomainDrivenDesignModule)
)]
public class FakeAspNetCoreModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpContextAccessor();
        context.Services.AddSingleton<ICurrentPrincipalAccessor, HttpContextCurrentPrincipalAccessor>();

        context.Services.AddObjectAccessor<IApplicationBuilder>();

        context.Services.AddTransient<IAuthorizationExceptionHandler, DefaultAuthorizationExceptionHandler>();
        context.Services.AddTransient<IHttpClientInfoProvider, HttpClientInfoProvider>();
        context.Services.AddTransient<FakeExceptionHandlingMiddleware>();

        context.Services.AddAuthorization();

        context.Services.AddSingleton<IAspNetCoreFileProvider, AspNetCoreFileProvider>();
        context.Services.Configure<FakeAspNetCoreFileOptions>(_ => { });

        context.Services.Configure<FakeVirtualFileSystemOptions>(options =>
        {
            options.FileProviders.Add<FakeAspNetCoreModule>("Fake.AspNetCore");
        });
        context.Services.Configure<FakeLocalizationOptions>(options =>
        {
            options.Resources.Add<FakeAspNetCoreErrorResource>("zh")
                .LoadVirtualJson("/Localization");
        });

        ConfigureControllers(context);
    }

    public override void ConfigureApplication(ApplicationConfigureContext context)
    {
        var environment = context.GetEnvironmentOrNull();

        if (environment != null)
        {
            // todo：important- WebRootFileProvider是UseStaticFiles工作的关键

            // 由 原WebRootFileProvider、wwwroot静态文件系统、Fake虚拟文件系统 组合
            environment.WebRootFileProvider = new CompositeFileProvider(
                environment.WebRootFileProvider,
                context.ServiceProvider.GetRequiredService<IAspNetCoreFileProvider>()
            );
        }
    }

    private void ConfigureControllers(ServiceConfigurationContext context)
    {
        context.Services.AddControllers()
            /*
             * important：
             *   默认使用的IControllerActivator是用ActivatorUtilities.CreateFactory创建实例。
             *   使用ServiceBasedControllerActivator替代，是交由HttpContext.RequestServices服务商提供。
             *   而Autofac的拦截器正是依赖于服务商才能生效。
             */
            .AddControllersAsServices();

        // Add feature providers
        var partManager = context.Services.GetInstance<ApplicationPartManager>();
        var application = context.Services.GetInstance<IFakeApplication>();
        partManager.FeatureProviders.Add(new RemoteServiceControllerFeatureProvider(application));
        partManager.ApplicationParts.TryAdd(new AssemblyPart(typeof(FakeAspNetCoreModule).Assembly));

        context.Services.AddTransient<IActionDescriptorProvider, ApplicationServiceActionDescriptorProvider>();
        context.Services.AddTransient<IApiDescriptionProvider, ApplicationServiceApiDescriptionProvider>();
        context.Services.AddOptions<MvcOptions>()
            .Configure(options =>
            {
                var conventionOptions =
                    context.Services.GetRequiredService<IOptions<ApplicationService2ControllerOptions>>();
                var actionConventional = context.Services.GetRequiredService<IApplicationServiceActionHelper>();

                options.Conventions.Add(new RemoteServiceConvention(conventionOptions, actionConventional));
            });
        context.Services.AddTransient<IApplicationServiceActionHelper, ApplicationServiceActionHelper>();
    }
}