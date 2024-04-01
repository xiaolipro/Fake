using Fake.DependencyInjection;

namespace Fake.SqlSugarCore;

[ExposeKeyedService<IDbContextProvider<TDbContext>>()]
public interface ISugarDbContextProvider<TDbContext> : IDbContextProvider<TDbContext> where TDbContext : SugarDbContext
{
    Task<TDbContext> GetDbContextAsync();
}