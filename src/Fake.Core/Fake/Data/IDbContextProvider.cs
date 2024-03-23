namespace Fake.Data;

public interface IDbContextProvider<TDbContext> where TDbContext : class
{
    Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default);
}