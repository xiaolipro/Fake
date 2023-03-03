using System.Threading.Tasks;
using Fake.UnitOfWork;

namespace Fake.EntityFrameworkCore;

public interface IEFCoreDbContextProvider<TDbContext> where TDbContext : IEFCoreDbContext
{
    Task<TDbContext> GetDbContextAsync();
}