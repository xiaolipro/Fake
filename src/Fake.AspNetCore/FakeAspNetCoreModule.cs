using Fake.AspNetCore.ExceptionHandling;
using Fake.AspNetCore.Security.Claims;
using Fake.Auditing;
using Fake.Identity;
using Fake.Identity.Security.Claims;
using Fake.Modularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.AspNetCore;

[DependsOn(
    typeof(FakeIdentityModule),
    typeof(FakeAuditingModule),
    typeof(FakeUnitOfWorkModule))]
public class FakeAspNetCoreModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpContextAccessor();
        context.Services.AddSingleton<ICurrentPrincipalAccessor, HttpContextCurrentPrincipalAccessor>();
        
        context.Services.AddObjectAccessor<IApplicationBuilder>();

        context.Services.AddTransient<IAuthorizationExceptionHandler, DefaultAuthorizationExceptionHandler>();
        
        context.Services.Configure<FakeAuditingOptions>(options => { });
    }
}