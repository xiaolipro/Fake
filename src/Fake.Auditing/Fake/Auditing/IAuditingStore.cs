using System.Threading.Tasks;

namespace Fake.Auditing;

public interface IAuditingStore
{
    Task SaveAsync(AuditLogInfo auditInfo);
}