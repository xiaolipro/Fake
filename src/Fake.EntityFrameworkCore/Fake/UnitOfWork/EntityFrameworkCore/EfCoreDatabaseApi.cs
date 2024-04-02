using Fake.EntityFrameworkCore;

namespace Fake.UnitOfWork.EntityFrameWorkCore;

public class EfCoreDatabaseApi<TDbContext> : IDatabaseApi, ISupportSaveChanges
    where TDbContext : EfCoreDbContext<TDbContext>
{
    public TDbContext DbContext { get; }

    public EfCoreDatabaseApi(TDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public bool HasChanges => DbContext.ChangeTracker.HasChanges();

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return DbContext.SaveChangesAsync(cancellationToken);
    }
}