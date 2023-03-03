using Fake.Auditing;
using Fake.Identity;
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
        context.Services.AddObjectAccessor<IApplicationBuilder>();
        
        context.Services.Configure<FakeAuditingOptions>(options =>
        {
            
        });
    }
}