﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Fake.EntityFrameworkCore;
using Fake.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fake.UnitOfWork.EntityFrameWorkCore;

/// <summary>
/// 基于工作单元的DbContext
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
public class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
    where TDbContext : FakeDbContext<TDbContext>
{
    public ILogger<UnitOfWorkDbContextProvider<TDbContext>> Logger { get; set; }

    private const string TransactionsNotSupportedErrorMessage = "当前数据库不支持事务！";
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;
    private readonly IConfiguration _configuration;

    public UnitOfWorkDbContextProvider(IUnitOfWorkManager unitOfWorkManager,
        ICancellationTokenProvider cancellationTokenProvider, IConfiguration configuration)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _cancellationTokenProvider = cancellationTokenProvider;
        _configuration = configuration;

        Logger = NullLogger<UnitOfWorkDbContextProvider<TDbContext>>.Instance;
    }

    public async Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
    {
        var unitOfWork = _unitOfWorkManager.Current;

        if (unitOfWork == null)
        {
            throw new FakeException("UnitOfWorkDbContext必须在工作单元内创建！");
        }

        var targetDbContextType = typeof(TDbContext);
        var connectionString = DbContextCreationContext.GetCreationContext<TDbContext>(_configuration).ConnectionString;

        var dbContextKey = $"{targetDbContextType.FullName}_{connectionString}";
        var databaseApi = unitOfWork.FindDatabaseApi(dbContextKey);

        if (databaseApi == null)
        {
            var dbContext = await CreateDbContextAsync(unitOfWork, connectionString, cancellationToken);
            databaseApi = new EfCoreDatabaseApi<TDbContext>(dbContext);

            unitOfWork.AddDatabaseApi(dbContextKey, databaseApi);
        }

        return ((EfCoreDatabaseApi<TDbContext>)databaseApi).DbContext;
    }


    private async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork,
        string connectionString, CancellationToken cancellationToken = default)
    {
        var creationContext = new DbContextCreationContext(connectionString);
        using (DbContextCreationContext.Use(creationContext))
        {
            var dbContext = unitOfWork.Context.IsTransactional
                ? await CreateDbContextWithTransactionAsync(unitOfWork, cancellationToken)
                // 这里必须直接注入FakeDbContext<TDbContext>，不能用IDbContextFactory<>，否则会报错
                : unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            dbContext.Initialize(unitOfWork);
            return dbContext;
        }
    }


    private async Task<TDbContext> CreateDbContextWithTransactionAsync(IUnitOfWork unitOfWork,
        CancellationToken cancellationToken = default)
    {
        var transactionApiKey = $"EntityFrameworkCore_{DbContextCreationContext.Current.ConnectionString}";
        var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as EfCoreTransactionApi;

        var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

        // 该连接未开启事务
        if (activeTransaction == null)
        {
            try
            {
                var token = GetCancellationToken(cancellationToken);
                // 开启数据库事务
                var dbTransaction = await dbContext.Database.BeginTransactionAsync(
                    unitOfWork.Context.IsolationLevel,
                    token
                );

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

            var token = GetCancellationToken(cancellationToken);

            // 如果是关系型数据库
            if (dbContext.Database.GetService<IDbContextTransactionManager>() is IRelationalTransactionManager)
            {
                // 同一个连接复用一个事务
                if (dbContext.Database.GetDbConnection() == DbContextCreationContext.Current.ExistingConnection)
                {
                    // 使当前数据库操作使用已开启的事务
                    await dbContext.Database.UseTransactionAsync(
                        activeTransaction.DbContextTransaction.GetDbTransaction(),
                        token
                    );
                }
                else
                {
                    try
                    {
                        /* 用户没有使用已存在的连接，我们将启动一个新的事务
                         * EfCoreTransactionApi将检查连接字符串匹配并通过DbContext实例提交/回滚
                         * */
                        await dbContext.Database.BeginTransactionAsync(
                            unitOfWork.Context.IsolationLevel,
                            token
                        );
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
                    await dbContext.Database.BeginTransactionAsync(token);
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