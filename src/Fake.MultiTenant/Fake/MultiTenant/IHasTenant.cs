using System;

namespace Fake.MultiTenant;

public interface IHasTenant
{
    /// <summary>
    /// 租户id
    /// </summary>
    Guid TenantId { get; }
}