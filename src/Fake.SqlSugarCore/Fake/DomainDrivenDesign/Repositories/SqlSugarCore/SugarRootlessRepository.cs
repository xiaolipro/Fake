using Fake.DependencyInjection;
using Fake.SqlSugarCore;

namespace Fake.DomainDrivenDesign.Repositories.SqlSugarCore;

public class SugarRootlessRepository<TDbContext> : IRootlessRepository
    where TDbContext : SugarDbContext<TDbContext>
{
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public ILazyServiceProvider LazyServiceProvider { get; set; } = default!; // 属性注入

    private ISugarDbContextProvider<TDbContext> EfDbContextProvider =>
        LazyServiceProvider.GetRequiredService<ISugarDbContextProvider<TDbContext>>();

    public async Task<ISqlSugarClient> GetDbContextAsync(CancellationToken cancellationToken = default)
    {
        var context = await EfDbContextProvider.GetDbContextAsync(cancellationToken);
        return context.SqlSugarClient;
    }
}