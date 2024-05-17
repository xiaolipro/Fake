using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Fake.Auditing;

public class SimpleAuditingStore(ILogger<SimpleAuditingStore> logger) : IAuditingStore
{
    public Task SaveAsync(AuditLogInfo auditInfo)
    {
        logger.LogInformation("{Log}", auditInfo.ToString());
        return Task.FromResult(0);
    }
}