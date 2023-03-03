using System.Threading.Tasks;
using Fake.UnitOfWork;

namespace Fake.EntityFrameworkCore;

public interface IEFCoreDbContextProvider<TDbContext> where TDbContext : IFakeDbContext
{
    Task<IUnitOfWork> GetDbContextAsync();
}