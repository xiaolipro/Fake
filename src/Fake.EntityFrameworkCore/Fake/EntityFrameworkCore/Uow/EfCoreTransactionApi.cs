using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fake.Threading;
using Fake.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fake.EntityFrameworkCore.Uow;

/// <summary>
/// EntityFrameworkCore事务api
/// </summary>
public class EfCoreTransactionApi : ITransactionApi, ISupportRollback
{
    public IDbContextTransaction DbContextTransaction { get; }
    public DbContext StarterDbContext { get; }
    public List<DbContext> AttendedDbContexts { get; }

    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    public EfCoreTransactionApi(
        IDbContextTransaction dbContextTransaction,
        DbContext starterDbContext,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        DbContextTransaction = dbContextTransaction;
        StarterDbContext = starterDbContext;
        CancellationTokenProvider = cancellationTokenProvider;
        AttendedDbContexts = new List<DbContext>();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        foreach (var dbContext in AttendedDbContexts)
        {
            // 关系型数据库如果使用相同连接，则会共享事务，不需要重复提交
            if (IsRelationalTransactionManagerAndSharedConnection(dbContext)) continue;

            await dbContext.Database.CommitTransactionAsync(CancellationTokenProvider.Token);
        }

        await DbContextTransaction.CommitAsync(CancellationTokenProvider.Token);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        foreach (var dbContext in AttendedDbContexts)
        {
            // 关系型数据库如果使用相同连接，则会共享事务，不需要重复提交
            if (IsRelationalTransactionManagerAndSharedConnection(dbContext)) continue;

            await dbContext.Database.RollbackTransactionAsync(
                CancellationTokenProvider.FallbackToProvider(cancellationToken));
        }

        await DbContextTransaction.RollbackAsync(CancellationTokenProvider.FallbackToProvider(cancellationToken));
    }

    private bool IsRelationalTransactionManagerAndSharedConnection(DbContext dbContext)
    {
        if (dbContext.Database.GetService<IDbContextTransactionManager>() is not IRelationalTransactionManager)
            return false;
        return dbContext.Database.GetDbConnection() == DbContextTransaction.GetDbTransaction().Connection;
    }

    public void Dispose()
    {
        DbContextTransaction.Dispose();
    }
}