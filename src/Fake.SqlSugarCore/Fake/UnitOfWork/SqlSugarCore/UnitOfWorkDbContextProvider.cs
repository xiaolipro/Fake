using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Fake.Data;
using Fake.SqlSugarCore;
using Fake.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fake.UnitOfWork.SqlSugarCore;

/// <summary>
/// 基于工作单元的DbContext
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
public class UnitOfWorkDbContextProvider<TDbContext>(
    IUnitOfWorkManager unitOfWorkManager,
    ICancellationTokenProvider cancellationTokenProvider,
    IConfiguration configuration)
    : IDbContextProvider<TDbContext>
    where TDbContext : SugarDbContext
{
    public readonly ILogger<UnitOfWorkDbContextProvider<TDbContext>> Logger =
        NullLogger<UnitOfWorkDbContextProvider<TDbContext>>
            .Instance;

    public async Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
    {
        var unitOfWork = unitOfWorkManager.Current;

        if (unitOfWork == null)
        {
            throw new FakeException($"{typeof(TDbContext).Name}必须在工作单元内创建！");
        }

        var targetDbContextType = typeof(TDbContext);
        var connectionString = DbContextCreationContext.GetCreationContext<TDbContext>(configuration).ConnectionString;

        var dbContextKey = $"{targetDbContextType.FullName}_{connectionString}";
        var databaseApi = unitOfWork.FindDatabaseApi(dbContextKey);

        if (databaseApi == null)
        {
            var dbContext = await CreateDbContextAsync(unitOfWork, connectionString, cancellationToken);
            databaseApi = new SqlSugarDatabaseApi<TDbContext>(dbContext);

            unitOfWork.AddDatabaseApi(dbContextKey, databaseApi);
        }

        return ((SqlSugarDatabaseApi<TDbContext>)databaseApi).DbContext;
    }


    private async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork,
        string connectionString, CancellationToken cancellationToken = default)
    {
        var creationContext = new DbContextCreationContext(connectionString);
        using (DbContextCreationContext.Use(creationContext))
        {
            var dbContext = unitOfWork.Context.IsTransactional
                ? await CreateDbContextWithTransactionAsync(unitOfWork, cancellationToken)
                : unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            dbContext.Initialize(unitOfWork);
            return dbContext;
        }
    }


    private async Task<TDbContext> CreateDbContextWithTransactionAsync(IUnitOfWork unitOfWork,
        CancellationToken cancellationToken = default)
    {
        Debug.Assert(DbContextCreationContext.Current != null, "DbContextCreationContext.Current != null");

        //事务key
        var transactionApiKey = $"SqlSugarCore_{DbContextCreationContext.Current.ConnectionString}";

        //尝试查找事务
        var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as SqlSugarTransactionApi;

        //该db还没有进行开启事务
        if (activeTransaction == null)
        {
            //获取到db添加事务即可
            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
            var transaction = new SqlSugarTransactionApi(
                dbContext
            );
            unitOfWork.AddTransactionApi(transactionApiKey, transaction);

            await dbContext.SqlSugarClient.Ado.BeginTranAsync();
            return dbContext;
        }

        return (TDbContext)activeTransaction.GetDbContext();
    }

    protected virtual CancellationToken GetCancellationToken(CancellationToken preferredValue = default)
    {
        return cancellationTokenProvider.FallbackToProvider(preferredValue);
    }
}