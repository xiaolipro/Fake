using Fake.Domain.Exceptions;
using Fake.EventBus.Local;
using Fake.TenantManagement.Domain.Events;
using Fake.TenantManagement.Domain.Localization;
using Fake.TenantManagement.Domain.TenantAggregate;

namespace Fake.TenantManagement.Domain.Services;

public class TenantManager(ITenantRepository tenantRepository, ILocalEventBus localEventBus)
    : TenantManagementDomainServiceBase
{
    public virtual async Task<Tenant> CreateAsync(string name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        var tenant = await tenantRepository.FindByNameAsync(name);

        if (tenant != null)
        {
            throw new DomainException(L[FakeTenantManagementResource.TenantNameDuplicate, name]);
        }

        return new Tenant(name);
    }

    public virtual async Task ChangeNameAsync(Tenant tenant, string name)
    {
        ArgumentNullException.ThrowIfNull(tenant, nameof(tenant));
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        var existedTenant = await tenantRepository.FindByNameAsync(name);

        if (existedTenant != null && existedTenant.Id != tenant.Id)
        {
            throw new DomainException(L[FakeTenantManagementResource.TenantNameDuplicate, name]);
        }

        await localEventBus.PublishAsync(new TenantNameChangedEvent(tenant.Id, tenant.Name, name));
        tenant.SetName(name);
    }
}