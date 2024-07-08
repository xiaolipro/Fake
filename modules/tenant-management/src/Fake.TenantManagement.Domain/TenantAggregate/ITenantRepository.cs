using Fake.Domain.Repositories;

namespace Fake.TenantManagement.Domain.TenantAggregate;

public interface ITenantRepository : IRepository<Tenant, Guid>
{
    Task<Tenant?> FindByNameAsync(string normalizedName);
}