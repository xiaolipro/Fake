using Fake.Data;
using Fake.DomainDrivenDesign.Entities.Auditing;

namespace Fake.TenantManagement.Domain.TenantAggregate;

public class Tenant : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; private set; } = null!;

    public List<TenantConnectionString> TenantConnectionStrings { get; private set; }

    public Tenant(string name)
    {
        SetName(name);
        TenantConnectionStrings = new List<TenantConnectionString>();
    }

    public void SetName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        Name = name;
    }

    public string? GetDefaultConnectionString()
    {
        return GetConnectionString(ConnectionStrings.DefaultConnectionStringName);
    }

    public string? GetConnectionString(string name)
    {
        return TenantConnectionStrings.FirstOrDefault(c => c.Name == name)?.Value;
    }

    public void SetDefaultConnectionString(string connectionString)
    {
        SetConnectionString(ConnectionStrings.DefaultConnectionStringName, connectionString);
    }

    public void SetConnectionString(string name, string connectionString)
    {
        var tenantConnectionString = TenantConnectionStrings.FirstOrDefault(x => x.Name == name);

        if (tenantConnectionString != null)
        {
            tenantConnectionString.SetValue(connectionString);
        }
        else
        {
            tenantConnectionString = new TenantConnectionString(Id, name, connectionString);
            TenantConnectionStrings.Add(tenantConnectionString);
        }
    }
}