using Fake.DependencyInjection;
using Fake.Timing;
using Microsoft.Extensions.Options;

namespace Fake.Auditing;

public class AuditingHelper : IAuditingHelper, ITransientDependency
{
    private readonly IClock _clock;
    private readonly AuditingOptions _options;

    public AuditingHelper(IOptions<AuditingOptions> options, IClock clock)
    {
        _clock = clock;
        _options = options.Value;
    }

    public AuditLogInfo CreateAuditLogInfo()
    {
        return new AuditLogInfo()
        {
            ApplicationName = _options.ApplicationName,
            ExecutionTime = _clock.Now,
        };
    }
}