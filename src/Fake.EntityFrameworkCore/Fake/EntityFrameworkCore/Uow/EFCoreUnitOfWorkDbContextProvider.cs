using System.Threading.Tasks;
using Fake.UnitOfWork;
using Microsoft.Extensions.Options;

namespace Fake.EntityFrameworkCore.Uow;

public class EFCoreUnitOfWorkDbContextProvider<TDbContext> : IEFCoreDbContextProvider<TDbContext>
    where TDbContext : IEFCoreDbContext
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
}