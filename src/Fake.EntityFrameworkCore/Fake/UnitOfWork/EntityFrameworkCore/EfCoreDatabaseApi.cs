using System.Threading;
using System.Threading.Tasks;
using Fake.EntityFrameworkCore;
using Fake.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Fake.UnitOfWork.EntityFrameWorkCore;

public class EfCoreDatabaseApi<TDbContext>: IDatabaseApi, ISupportSaveChanges
    where TDbContext:FakeDbContext<TDbContext>
{
    public TDbContext DbContext { get; }

    public EfCoreDatabaseApi(TDbContext dbContext)
    {
        DbContext = dbContext;
    }
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return DbContext.SaveChangesAsync(cancellationToken);
    }
}