using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public interface IAmbientUnitOfWorkProvider:IUnitOfWorkAccessor
{
    [CanBeNull]
    IUnitOfWork GetCurrentByChecking();
}