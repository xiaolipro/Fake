using System;

namespace Fake.MultiTenant;

public class TenantInfo(Guid tenantId, string name)
{
    public Guid TenantId { get; } = tenantId;

    public string Name { get; } = name;
}