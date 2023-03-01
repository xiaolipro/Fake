using System.Threading;
using System.Threading.Tasks;
using Fake.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Fake.EntityFrameworkCore.Uow;

public class EFCoreDatabaseApi: IDatabaseApi, ISupportSaveChanges
{
    public DbContext DbContext { get; }

    public EFCoreDatabaseApi(DbContext dbContext)
    {
        DbContext = dbContext;
    }
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return DbContext.SaveChangesAsync(cancellationToken);
    }
}