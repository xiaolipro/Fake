using System;
using System.Threading.Tasks;
using Fake.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fake.Auditing;

[Dependency(TryAdd = true)]
public class SimpleLogAuditingStore:IAuditingStore
{
    public ILogger<SimpleLogAuditingStore> Logger { get; set; }
    
    public SimpleLogAuditingStore()
    {
        Logger = NullLogger<SimpleLogAuditingStore>.Instance;
    }
    
    public Task SaveAsync(AuditLogInfo auditInfo)
    {
        Logger.LogInformation(auditInfo.ToString());
        Console.WriteLine(auditInfo.ToString());
        return Task.FromResult(0);
    }
}