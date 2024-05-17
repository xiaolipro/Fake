using System;

namespace Fake.Auditing;

public class AuditLogContributionContext(IServiceProvider serviceProvider, AuditLogInfo auditInfo)
    : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    public AuditLogInfo AuditInfo { get; } = auditInfo;
}