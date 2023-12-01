namespace Fake.Auditing;

public interface IAuditingManager
{
    IAuditLogScope? Current { get; }

    IAuditLogSaveHandle BeginScope();
}