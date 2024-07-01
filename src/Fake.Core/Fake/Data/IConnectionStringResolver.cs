using Microsoft.Extensions.Configuration;

namespace Fake.Data;

public interface IConnectionStringResolver
{
    Task<string> ResolveAsync(string connectionStringName);
}

public class DefaultConnectionStringResolver(IConfiguration configuration) : IConnectionStringResolver
{
    public Task<string> ResolveAsync(string connectionStringName)
    {
        if (connectionStringName == null) throw new ArgumentNullException(nameof(connectionStringName));
        var connectionString = configuration.GetConnectionString(connectionStringName);

        return Task.FromResult(connectionString!);
    }
}