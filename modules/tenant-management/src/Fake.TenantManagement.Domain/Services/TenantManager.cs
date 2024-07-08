using Fake.Domain;
using Fake.Domain.Services;
using Fake.EventBus.Local;
using Fake.TenantManagement.Domain.Events;
using Fake.TenantManagement.Domain.Localization;
using Fake.TenantManagement.Domain.TenantAggregate;

namespace Fake.TenantManagement.Domain.Services;

public class TenantManager(ITenantRepository tenantRepository, ILocalEventBus localEventBus) : DomainService
{
    public virtual async Task<Tenant> CreateAsync(string name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        var tenant = await tenantRepository.FindByNameAsync(name);

        if (tenant != null)
        {
            throw new DomainException("TenantManagement:DuplicateTenantName");
        }

        return new Tenant(name);
    }

    public virtual async Task ChangeNameAsync(Tenant tenant, string name)
    {
        ArgumentNullException.ThrowIfNull(tenant, nameof(tenant));
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        var tenant2 = await tenantRepository.FindByNameAsync(name);

        if (tenant2 != null && tenant2.Id != tenant.Id)
        {
            throw new DomainException(FakeTenantManagementResource.TenantNameDuplicate);
        }

        await localEventBus.PublishAsync(new TenantNameChangedEvent(tenant.Id, tenant.Name, name));
        tenant.SetName(name);
    }
}