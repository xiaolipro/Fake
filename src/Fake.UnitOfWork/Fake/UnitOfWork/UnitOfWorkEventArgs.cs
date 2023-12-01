namespace Fake.UnitOfWork;

public class UnitOfWorkEventArgs
{
    public IUnitOfWork UnitOfWork { get; }

    public UnitOfWorkEventArgs(IUnitOfWork unitOfWork)
    {
        ThrowHelper.ThrowIfNull(unitOfWork);

        UnitOfWork = unitOfWork;
    }
}