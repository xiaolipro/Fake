using System.Threading.Tasks;
using Fake.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Fake.EntityFrameworkCore;

public interface IEFCoreDbContextProvider<TDbContext> where TDbContext : DbContext
{
    Task<TDbContext> GetDbContextAsync();
}