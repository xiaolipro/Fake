namespace Fake.UnitOfWork;

public interface IUnitOfWorkTransactionalProvider
{
    bool IsTransactional { get; }
}