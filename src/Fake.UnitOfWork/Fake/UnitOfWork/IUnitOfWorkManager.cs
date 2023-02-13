using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public interface IUnitOfWorkManager
{
    [CanBeNull] IUnitOfWork Current { get; }
    
    IUnitOfWork BeginScope();
}