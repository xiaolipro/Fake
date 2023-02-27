using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public class UnitOfWorkEventArgs
{
    public IUnitOfWork UnitOfWork { get; }

    public UnitOfWorkEventArgs([NotNull] IUnitOfWork unitOfWork)
    {
        ThrowHelper.ThrowIfNull(unitOfWork);

        UnitOfWork = unitOfWork;
    }
}