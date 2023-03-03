using System.Threading;
using System.Threading.Tasks;
using Fake.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Fake.EntityFrameworkCore.Uow;

public class EFCoreDatabaseApi: IDatabaseApi, ISupportSaveChanges
{
    public IEFCoreDbContext DbContext { get; }

    public EFCoreDatabaseApi(IEFCoreDbContext dbContext)
    {
        DbContext = dbContext;
    }
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return DbContext.SaveChangesAsync(cancellationToken);
    }
}