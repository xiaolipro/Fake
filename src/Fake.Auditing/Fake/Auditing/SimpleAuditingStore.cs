using System.Threading.Tasks;
using Fake.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fake.Auditing;

[Dependency(TryAdd = true)]
public class SimpleAuditingStore(ILogger<SimpleAuditingStore> logger) : IAuditingStore
{
    public Task SaveAsync(AuditLogInfo auditInfo)
    {
        logger.LogInformation("{Log}", auditInfo.ToString());
        return Task.FromResult(0);
    }
}