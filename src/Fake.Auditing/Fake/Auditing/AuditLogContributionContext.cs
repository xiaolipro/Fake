using System;
using Fake.Core.DependencyInjection;

namespace Fake.Auditing;

public class AuditLogContributionContext: IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }
    
    public AuditLogInfo AuditInfo { get; }
    
    public AuditLogContributionContext(IServiceProvider serviceProvider, AuditLogInfo auditInfo)
    {
        ServiceProvider = serviceProvider;
        AuditInfo = auditInfo;
    }
}