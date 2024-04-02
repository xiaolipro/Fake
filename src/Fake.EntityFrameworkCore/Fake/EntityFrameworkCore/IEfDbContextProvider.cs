namespace Fake.EntityFrameworkCore;

public interface IEfDbContextProvider<TDbContext>
    where TDbContext : EfCoreDbContext<TDbContext>
{
    Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default);
}