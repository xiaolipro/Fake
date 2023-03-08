using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public interface IUnitOfWorkAccessor
{
    [CanBeNull]
    IUnitOfWork UnitOfWork { get; }

    void SetUnitOfWork([CanBeNull] IUnitOfWork unitOfWork);
}