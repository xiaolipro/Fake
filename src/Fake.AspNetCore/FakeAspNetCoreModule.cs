using Fake.Auditing;
using Fake.Identity;
using Fake.Modularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.AspNetCore;

[DependsOn(
    typeof(FakeIdentityModuleApplication),
    typeof(FakeAuditingModuleApplication),
    typeof(FakeUnitOfWorkModuleApplication))]
public class FakeAspNetCoreModule : FakeModuleApplication
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpContextAccessor();
        context.Services.AddObjectAccessor<IApplicationBuilder>();
        
        context.Services.Configure<FakeAuditingOptions>(options =>
        {
            
        });
    }
}