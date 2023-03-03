using System;
using System.Threading;
using System.Threading.Tasks;
using Fake.Threading;
using Fake.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Fake.EntityFrameworkCore.UnitOfWork;

public class EfCoreUnitOfWorkDbContextProvider<TDbContext> : IEfCoreDbContextProvider<TDbContext>
    where TDbContext : DbContext
{
    public ILogger<EfCoreUnitOfWorkDbContextProvider<TDbContext>> Logger { get; set; }

    private const string TransactionsNotSupportedErrorMessage = "当前数据库不支持事务！";
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;
    private readonly EfCoreOptions _options;

    public EfCoreUnitOfWorkDbContextProvider(IUnitOfWorkManager unitOfWorkManager,
        ICancellationTokenProvider cancellationTokenProvider,
        IOptions<EfCoreOptions> options)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _cancellationTokenProvider = cancellationTokenProvider;
        _options = options.Value;

        Logger = NullLogger<EfCoreUnitOfWorkDbContextProvider<TDbContext>>.Instance;
    }

    public async Task<TDbContext> GetDbContextAsync()
    {
        var unitOfWork = _unitOfWorkManager.Current;

        if (unitOfWork == null)
        {
            throw new FakeException("UnitOfWorkDbContext必须在工作单元内创建！");
        }

        var targetDbContextType = typeof(TDbContext);
        var connectionStringName = ConnectionStringNameAttribute.GetConnStringName(targetDbContextType);
        var connectionString = "await ResolveConnectionStringAsync(connectionStringName)";

        var dbContextKey = $"{targetDbContextType.FullName}_{connectionString}";
        var databaseApi = unitOfWork.FindDatabaseApi(dbContextKey);

        if (databaseApi == null)
        {
            var dbContext = await CreateDbContextAsync(unitOfWork,connectionStringName, connectionString);
            databaseApi = new EfCoreDatabaseApi(dbContext);

            unitOfWork.AddDatabaseApi(dbContextKey, databaseApi);
        }

        return (TDbContext)((EfCoreDatabaseApi)databaseApi).DbContext;
    }


    private async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork, string connectionStringName,
        string connectionString)
    {
        var creationContext = new DbContextCreationContext(connectionStringName, connectionString);
        using (DbContextCreationContext.Use(creationContext))
        {
            var dbContext = unitOfWork.Context.IsTransactional
                ? await CreateDbContextWithTransactionAsync(unitOfWork)
                : unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            if (dbContext is FakeDbContext<TDbContext> fakeDbContext)
            {
                fakeDbContext.Initialize(unitOfWork);
            }

            return dbContext;
        }
    }


    private async Task<TDbContext> CreateDbContextWithTransactionAsync(IUnitOfWork unitOfWork)
    {
        var transactionApiKey = $"EntityFrameworkCore_{DbContextCreationContext.Current.ConnectionString}";
        var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as EfCoreTransactionApi;

        var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

        // 该连接未开启事务
        if (activeTransaction == null)
        {
            try
            {
                // 开启数据库事务
                var dbTransaction = unitOfWork.Context.IsolationLevel.HasValue
                    ? await dbContext.Database.BeginTransactionAsync(unitOfWork.Context.IsolationLevel.Value,
                        GetCancellationToken())
                    : await dbContext.Database.BeginTransactionAsync(GetCancellationToken());

                unitOfWork.AddTransactionApi(
                    transactionApiKey,
                    new EfCoreTransactionApi(
                        dbTransaction,
                        dbContext,
                        _cancellationTokenProvider
                    )
                );
            }
            catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
            {
                Logger.LogError(TransactionsNotSupportedErrorMessage);
                Logger.LogException(e);

                return dbContext;
            }
        }
        else
        {
            DbContextCreationContext.Current.ExistingConnection =
                activeTransaction.DbContextTransaction.GetDbTransaction().Connection;

            // 如果是关系型数据库
            if (dbContext.Database.GetService<IDbContextTransactionManager>() is IRelationalTransactionManager)
            {
                // 同一个连接复用一个事务
                if (dbContext.Database.GetDbConnection() == DbContextCreationContext.Current.ExistingConnection)
                {
                    // 使当前数据库操作使用已开启的事务
                    await dbContext.Database.UseTransactionAsync(
                        activeTransaction.DbContextTransaction.GetDbTransaction(), GetCancellationToken());
                }
                else
                {
                    try
                    {
                        /* 用户没有使用已存在的连接，我们将启动一个新的事务
                         * EfCoreTransactionApi将检查连接字符串匹配并通过DbContext实例提交/回滚
                         * */
                        if (unitOfWork.Context.IsolationLevel.HasValue)
                        {
                            await dbContext.Database.BeginTransactionAsync(
                                unitOfWork.Context.IsolationLevel.Value,
                                GetCancellationToken()
                            );
                        }
                        else
                        {
                            await dbContext.Database.BeginTransactionAsync(
                                GetCancellationToken()
                            );
                        }
                    }
                    catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
                    {
                        Logger.LogError(TransactionsNotSupportedErrorMessage);
                        Logger.LogException(e);

                        return dbContext;
                    }
                }
            }
            else
            {
                try
                {
                    await dbContext.Database.BeginTransactionAsync(GetCancellationToken());
                }
                catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
                {
                    Logger.LogError(TransactionsNotSupportedErrorMessage);
                    Logger.LogException(e);

                    return dbContext;
                }
            }

            activeTransaction.AttendedDbContexts.Add(dbContext);
        }

        return dbContext;
    }

    protected virtual CancellationToken GetCancellationToken(CancellationToken preferredValue = default)
    {
        return _cancellationTokenProvider.FallbackToProvider(preferredValue);
    }
}