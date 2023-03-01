using Fake.DependencyInjection;
using Fake.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Fake.EntityFrameworkCore;

public interface IFakeDbContext
{
    void Initialize(IUnitOfWork unitOfWork);
}

public class FakeDbContext : DbContext, IFakeDbContext, ITransientDependency
{
    public void Initialize(IUnitOfWork unitOfWork)
    {
        throw new System.NotImplementedException();
    }
}