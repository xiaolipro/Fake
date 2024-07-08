using Fake.AspNetCore;
using Fake.AspNetCore.Mvc.Conventions;
using Fake.Modularity;
using Fake.ObjectMapping.AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.TenantManagement.Application;

[DependsOn(typeof(FakeObjectMappingAutoMapperModule))]
[DependsOn(typeof(FakeAspNetCoreModule))]
public class FakeTenantManagementApplicationModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<FakeAutoMapperOptions>(options =>
        {
            options.ScanProfiles<FakeTenantManagementApplicationModule>(true);
        });

        context.Services.Configure<RemoteService2ControllerOptions>(options =>
        {
            options.ScanRemoteServices<FakeTenantManagementApplicationModule>();
        });

        context.Services.AddFakeSwaggerGen();
    }

    public override void ConfigureApplication(ApplicationConfigureContext context)
    {
        var app = context.GetWebApplication();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseFakeSwagger();

        app.MapControllers();
    }
}