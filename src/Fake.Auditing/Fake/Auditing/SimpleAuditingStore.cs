using System;
using System.Threading.Tasks;
using Fake.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fake.Auditing;

[Dependency(TryAdd = true)]
public class SimpleAuditingStore:IAuditingStore
{
    private readonly ILogger<SimpleAuditingStore> _logger;
    
    public SimpleAuditingStore(ILogger<SimpleAuditingStore> logger)
    {
        _logger = logger;
    }
    
    public Task SaveAsync(AuditLogInfo auditInfo)
    {
        _logger.LogInformation(auditInfo.ToString());
        Console.WriteLine(auditInfo);
        return Task.FromResult(0);
    }
}