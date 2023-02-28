using System.Threading;
using System.Threading.Tasks;
using Fake.UnitOfWork;

namespace Fake.EntityFrameworkCore.Uow;

public class EfCoreDatabaseApi: IDatabaseApi, ISupportSaveChanges
{
    public IEfCoreDbContext DbContext { get; }

    public EfCoreDatabaseApi(IEfCoreDbContext dbContext)
    {
        DbContext = dbContext;
    }
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return DbContext.SaveChangesAsync(cancellationToken);
    }
}