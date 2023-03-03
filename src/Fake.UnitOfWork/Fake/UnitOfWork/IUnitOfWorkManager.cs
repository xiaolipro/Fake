using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public interface IUnitOfWorkManager
{
    [CanBeNull] IUnitOfWork Current { get; }

    IUnitOfWork Begin([CanBeNull] UnitOfWorkAttribute attribute, bool requiredNew = false);
}