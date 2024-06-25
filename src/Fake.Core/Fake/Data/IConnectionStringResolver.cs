using Microsoft.Extensions.Configuration;

namespace Fake.Data;

public interface IConnectionStringResolver
{
    Task<string> ResolveAsync(string? connectionStringName = null);
}

public class DefaultConnectionStringResolver(IConfiguration configuration) : IConnectionStringResolver
{
    public Task<string> ResolveAsync(string? connectionStringName = null)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName)!;
    }
}