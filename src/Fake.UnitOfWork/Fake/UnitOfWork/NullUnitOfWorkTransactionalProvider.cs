namespace Fake.UnitOfWork;

public class NullUnitOfWorkTransactionalProvider : IUnitOfWorkTransactionalProvider
{
    public bool IsTransactional => false;
}