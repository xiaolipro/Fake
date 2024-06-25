using System;
using Fake.Threading;

namespace Fake.MultiTenant;

public class CurrentTenant(IAmbientScopeProvider<TenantInfo> ambientScopeProvider) : ICurrentTenant
{
    private const string Key = "Fake.MultiTenant.TenantInfo";
    private TenantInfo? Current => ambientScopeProvider.GetValue(Key);

    public bool IsResolved => Id == default;
    public Guid Id => Current?.TenantId ?? default;
    public string Name => Current?.Name ?? string.Empty;

    public IDisposable Change(TenantInfo tenantInfo)
    {
        return ambientScopeProvider.BeginScope(Key, tenantInfo);
    }
}