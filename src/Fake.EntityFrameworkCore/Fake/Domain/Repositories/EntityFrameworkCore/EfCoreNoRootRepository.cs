using System.Threading;
using System.Threading.Tasks;
using Fake.DependencyInjection;
using Fake.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fake.Domain.Repositories.EntityFrameWorkCore;

public class EfCoreNoRootRepository<TDbContext> : IEfCoreNoRootRepository<TDbContext> where TDbContext : DbContext
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public ILazyServiceProvider LazyServiceProvider { get; set; } // 属性注入

    private IDbContextProvider<TDbContext> DbContextProvider =>
        LazyServiceProvider.GetRequiredLazyService<IDbContextProvider<TDbContext>>();

    public async Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
    {
        var context = await DbContextProvider.GetDbContextAsync(cancellationToken);
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        //context.SavedChanges += (_, _) => throw new FakeException("请不要在无根仓储中执行查询以外的操作！");

        return context;
    }
}