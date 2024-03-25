namespace Fake.SqlSugarCore;

public interface ISugarDbContextProvider<TDbContext> : IDbContextProvider<TDbContext> where TDbContext : SugarDbContext
{
    Task<TDbContext> GetDbContextAsync();
}