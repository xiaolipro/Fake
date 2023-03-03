using Fake.DependencyInjection;

namespace Fake.UnitOfWork;

public class NullUnitOfWorkTransactionalProvider : IUnitOfWorkTransactionalProvider, ISingletonDependency
{
    public bool IsTransactional => false;
}