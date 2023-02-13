using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public interface IUnitOfWorkManager
{
    [CanBeNull] IUnitOfWorkScope Current { get; }
    
    IAuditLogSaveHandle BeginScope();
}