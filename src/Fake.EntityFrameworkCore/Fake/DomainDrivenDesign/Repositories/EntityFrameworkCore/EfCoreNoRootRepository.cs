using System.Threading;
using System.Threading.Tasks;
using Fake.DependencyInjection;
using Fake.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fake.DomainDrivenDesign.Repositories.EntityFrameWorkCore;

public class EfCoreRootlessRepository<TDbContext> : IRootlessRepository
    where TDbContext : FakeDbContext<TDbContext>
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public ILazyServiceProvider LazyServiceProvider { get; set; } // 属性注入

    private IDbContextProvider<TDbContext> DbContextProvider =>
        LazyServiceProvider.GetRequiredService<IDbContextProvider<TDbContext>>();

    public async Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
    {
        var context = await DbContextProvider.GetDbContextAsync(cancellationToken);
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        return context;
    }
}