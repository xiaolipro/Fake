using Fake.SqlSugarCore;

namespace Fake.UnitOfWork.SqlSugarCore;

public class SqlSugarDatabaseApi<TDbContext> : IDatabaseApi
    where TDbContext : SugarDbContext
{
    public TDbContext DbContext { get; }

    public SqlSugarDatabaseApi(TDbContext dbContext)
    {
        DbContext = dbContext;
    }
}