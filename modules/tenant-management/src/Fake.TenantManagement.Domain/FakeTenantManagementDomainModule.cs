using Fake.DomainDrivenDesign;
using Fake.Localization;
using Fake.Modularity;
using Fake.TenantManagement.Domain.Localization;
using Fake.VirtualFileSystem;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.TenantManagement.Domain;

[DependsOn(typeof(FakeDomainDrivenDesignModule))]
public class FakeTenantManagementDomainModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<FakeVirtualFileSystemOptions>(options =>
        {
            options.FileProviders.Add<FakeTenantManagementDomainModule>();
        });

        context.Services.Configure<FakeLocalizationOptions>(options =>
        {
            options.Resources.Add<FakeTenantManagementResource>("zh")
                .LoadVirtualJson("/Localization/Resources");
        });
    }
}