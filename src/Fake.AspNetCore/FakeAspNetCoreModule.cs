using Fake.AspNetCore;
using Fake.AspNetCore.Auditing;
using Fake.AspNetCore.ExceptionHandling;
using Fake.AspNetCore.Http;
using Fake.AspNetCore.Security.Claims;
using Fake.AspNetCore.VirtualFileSystem;
using Fake.Auditing;
using Fake.Identity;
using Fake.Identity.Security.Claims;
using Fake.Modularity;
using Fake.UnitOfWork;
using Fake.VirtualFileSystem;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

[DependsOn(
    typeof(FakeIdentityModule),
    typeof(FakeAuditingModule),
    typeof(FakeUnitOfWorkModule),
    typeof(FakeVirtualFileSystemModule))]
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

        context.Services.Configure<FakeAuditingOptions>(options =>
        {
            options.Contributors.Add(new AspNetCoreAuditLogContributor());
        });

        context.Services.AddAuthorization();

        context.Services.AddSingleton<IAspNetCoreFileProvider, AspNetCoreFileProvider>();
        context.Services.Configure<FakeAspNetCoreFileOptions>(_ => { });
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
}