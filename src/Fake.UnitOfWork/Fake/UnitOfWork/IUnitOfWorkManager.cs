using Fake.DependencyInjection;
using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public interface IUnitOfWorkManager
{
    [CanBeNull] IUnitOfWork Current { get; }
    
    IUnitOfWork Begin(UnitOfWorkContext context, bool requiredNew);
}


public class UnitOfWorkManager : IUnitOfWorkManager, ISingletonDependency
{
    public IUnitOfWork Current { get; }
    public IUnitOfWork Begin(UnitOfWorkContext context, bool requiredNew)
    {
        throw new System.NotImplementedException();
    }
}