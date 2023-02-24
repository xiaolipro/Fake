using Fake.DependencyInjection;

namespace Fake.UnitOfWork;

public class NullUnitOfWorkIsTransactionalProvider : IUnitOfWorkIsTransactionalProvider, ISingletonDependency
{
    public bool? IsTransactional => null;
}