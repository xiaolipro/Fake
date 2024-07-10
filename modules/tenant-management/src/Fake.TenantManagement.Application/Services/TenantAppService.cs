using Fake.Domain;
using Fake.TenantManagement.Application.Contracts.Dtos;
using Fake.TenantManagement.Domain.Localization;
using Fake.TenantManagement.Domain.Services;
using Fake.TenantManagement.Domain.TenantAggregate;

namespace Fake.TenantManagement.Application.Services;

public class TenantAppService(ITenantRepository tenantRepository, TenantManager tenantManager)
    : TenantManagementAppServiceBase
{
    public async Task<TenantDto> GetAsync(Guid id)
    {
        var tenant = await tenantRepository.FirstOrDefaultAsync(id);

        if (tenant == null) throw new BusinessException(L[FakeTenantManagementResource.TenantNotExists, id]);

        return ObjectMapper.Map<Tenant, TenantDto>(tenant);
    }

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