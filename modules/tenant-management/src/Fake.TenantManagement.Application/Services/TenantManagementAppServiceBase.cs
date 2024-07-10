using Fake.Application;
using Fake.TenantManagement.Domain.Localization;

namespace Fake.TenantManagement.Application.Services;

public abstract class TenantManagementAppServiceBase : ApplicationService
{
    protected TenantManagementAppServiceBase()
    {
        LocalizationResource = typeof(FakeTenantManagementResource);
    }
}