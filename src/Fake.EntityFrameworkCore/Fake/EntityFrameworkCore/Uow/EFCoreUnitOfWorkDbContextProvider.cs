using System;
using System.Threading.Tasks;
using Fake.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fake.EntityFrameworkCore.Uow;

public class EFCoreUnitOfWorkDbContextProvider<TDbContext> : IEFCoreDbContextProvider<TDbContext>
    where TDbContext : DbContext
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly EFCoreDbContextOptions _options;

    public EFCoreUnitOfWorkDbContextProvider(IUnitOfWorkManager unitOfWorkManager,
        IOptions<EFCoreDbContextOptions> options)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _options = options.Value;
    }
    public async Task<TDbContext> GetDbContextAsync()
    {
        var unitOfWork = _unitOfWorkManager.Current;
        
        if (unitOfWork is null)
        {
            throw new FakeException("UnitOfWorkDbContext必须在工作单元内工作！");
        }
        var connectionStringName = "ConnectionStringNameAttribute.GetConnStringName(targetDbContextType)";
        var connectionString = "await ResolveConnectionStringAsync(connectionStringName)";

        var dbContextKey = $"targetDbContextType.FullName_{connectionString}";
        var databaseApi = unitOfWork.FindDatabaseApi(dbContextKey);
        
        if (databaseApi == null)
        {
            var dbContext = await CreateDbContextAsync(unitOfWork, connectionStringName, connectionString);
            databaseApi = new EFCoreDatabaseApi(dbContext);

            unitOfWork.AddDatabaseApi(dbContextKey, databaseApi);
        }

        return (TDbContext)((EFCoreDatabaseApi)databaseApi).DbContext;
    }
    
    
    private async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork, string connectionStringName, string connectionString)
    {
        var creationContext = new DbContextCreationContext(connectionStringName, connectionString);
        using (DbContextCreationContext.Use(creationContext))
        {
            var dbContext = await CreateDbContextAsync(unitOfWork);

            if (dbContext is IAbpEfCoreDbContext abpEfCoreDbContext)
            {
                abpEfCoreDbContext.Initialize(
                    new AbpEfCoreDbContextInitializationContext(
                        unitOfWork
                    )
                );
            }

            return dbContext;
        }
    }
    
    
    private async Task<TDbContext> CreateDbContextWithTransactionAsync(IUnitOfWork unitOfWork)
    {
        var transactionApiKey = $"EntityFrameworkCore_{DbContextCreationContext.Current.ConnectionString}";
        var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as EFCoreTransactionApi;

        if (activeTransaction == null)
        {
            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            try
            {
                var dbTransaction = unitOfWork.Options.IsolationLevel.HasValue
                    ? await dbContext.Database.BeginTransactionAsync(unitOfWork.Options.IsolationLevel.Value, GetCancellationToken())
                    : await dbContext.Database.BeginTransactionAsync(GetCancellationToken());

                unitOfWork.AddTransactionApi(
                    transactionApiKey,
                    new EFCoreTransactionApi(
                        dbTransaction,
                        dbContext,
                        _cancellationTokenProvider
                    )
                );
            }
            catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
            {
                Logger<>.LogError(TransactionsNotSupportedErrorMessage);
                Logger<>.LogException(e);
                    
                return dbContext;
            }

            return dbContext;
        }
        else
        {
            DbContextCreationContext.Current.ExistingConnection = activeTransaction.DbContextTransaction.GetDbTransaction().Connection;

            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            if (dbContext.As<DbContext>().HasRelationalTransactionManager())
            {
                if (dbContext.Database.GetDbConnection() == DbContextCreationContext.Current.ExistingConnection)
                {
                    await dbContext.Database.UseTransactionAsync(activeTransaction.DbContextTransaction.GetDbTransaction(), GetCancellationToken());
                }
                else
                {
                    try
                    {
                        /* User did not re-use the ExistingConnection and we are starting a new transaction.
                            * EfCoreTransactionApi will check the connection string match and separately
                            * commit/rollback this transaction over the DbContext instance. */
                        if (unitOfWork.Options.IsolationLevel.HasValue)
                        {
                            await dbContext.Database.BeginTransactionAsync(
                                unitOfWork.Options.IsolationLevel.Value,
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
                        Logger<>.LogError(TransactionsNotSupportedErrorMessage);
                        Logger<>.LogException(e);
                    
                        return dbContext;
                    }
                }
            }
            else
            {
                try
                {
                    /* No need to store the returning IDbContextTransaction for non-relational databases
                        * since EfCoreTransactionApi will handle the commit/rollback over the DbContext instance.
                          */
                    await dbContext.Database.BeginTransactionAsync(GetCancellationToken());
                }
                catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
                {
                    Logger<>.LogError(TransactionsNotSupportedErrorMessage);
                    Logger<>.LogException(e);
                    
                    return dbContext;
                }
            }

            activeTransaction.AttendedDbContexts.Add(dbContext);

            return dbContext;
        }
    }
}