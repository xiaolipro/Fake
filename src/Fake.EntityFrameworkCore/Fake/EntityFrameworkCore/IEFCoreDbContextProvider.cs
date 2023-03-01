using System.Threading.Tasks;

namespace Fake.EntityFrameworkCore;

public interface IEFCoreDbContextProvider<TDbContext> where TDbContext : IFakeDbContext
{
    Task<TDbContext> GetDbContextAsync();
}