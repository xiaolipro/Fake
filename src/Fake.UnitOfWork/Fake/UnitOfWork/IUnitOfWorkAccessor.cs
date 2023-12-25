namespace Fake.UnitOfWork;

public interface IUnitOfWorkAccessor
{
    IUnitOfWork? UnitOfWork { get; }

    void SetUnitOfWork(IUnitOfWork? unitOfWork);
}