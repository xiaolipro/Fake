using Fake.DependencyInjection;
using Fake.Domain.Repositories;
using Fake.EntityFrameworkCore;

namespace Fake.DomainDrivenDesign.Repositories.EntityFrameWorkCore;

public class EfCoreRootlessRepository<TDbContext> : IRootlessRepository
    where TDbContext : EfCoreDbContext<TDbContext>
{
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public ILazyServiceProvider LazyServiceProvider { get; set; } = default!; // 属性注入

    private IEfDbContextProvider<TDbContext> EfDbContextProvider =>
        LazyServiceProvider.GetRequiredService<IEfDbContextProvider<TDbContext>>();

    public async Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
    {
        var context = await EfDbContextProvider.GetDbContextAsync(cancellationToken);
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        return context;
    }
}