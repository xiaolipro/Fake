using System.Threading.Tasks;
using Fake.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fake.Domain.Repositories.EntityFrameWorkCore;

public class EfCoreRepository<TDbContext> where TDbContext : DbContext
{
    private readonly IDbContextProvider<TDbContext> _dbContextProvider;
    
    public EfCoreRepository(IDbContextProvider<TDbContext> dbContextProvider)
    {
        _dbContextProvider = dbContextProvider;
    }
    
    protected Task<TDbContext> GetDbContextAsync()
    {
        return _dbContextProvider.GetDbContextAsync();
    }
}