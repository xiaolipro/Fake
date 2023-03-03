using System.Threading.Tasks;
using Fake.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Fake.EntityFrameworkCore;

public interface IEfCoreDbContextProvider<TDbContext> where TDbContext : DbContext
{
    Task<TDbContext> GetDbContextAsync();
}