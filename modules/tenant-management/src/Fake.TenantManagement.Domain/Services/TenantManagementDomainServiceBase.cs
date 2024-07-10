using Fake.Domain;
using Fake.TenantManagement.Domain.Localization;

namespace Fake.TenantManagement.Domain.Services;

public abstract class TenantManagementDomainServiceBase : DomainService
{
    protected TenantManagementDomainServiceBase()
    {
        LocalizationResource = typeof(FakeTenantManagementResource);
    }
}