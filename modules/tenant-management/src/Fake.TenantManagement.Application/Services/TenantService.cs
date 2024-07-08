using Fake.TenantManagement.Application.Contracts.Services;
using Fake.TenantManagement.Domain.TenantAggregate;

namespace Fake.TenantManagement.Application.Services;

public class TenantService(ITenantRepository tenantRepository) : ITenantService
{
    public async Task<string> GetDefaultConnectionStringAsync(Guid id)
    {
        var tenant = await tenantRepository.FirstAsync(id);

        return tenant.GetDefaultConnectionString();
    }

    public Task UpdateDefaultConnectionStringAsync(Guid id, string defaultConnectionString)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDefaultConnectionStringAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}