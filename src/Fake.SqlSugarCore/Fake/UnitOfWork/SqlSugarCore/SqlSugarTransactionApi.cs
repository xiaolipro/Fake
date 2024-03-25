using System.Threading;
using Fake.SqlSugarCore;

namespace Fake.UnitOfWork.SqlSugarCore;

/// <summary>
/// SqlSugarCore事务api
/// </summary>
public class SqlSugarTransactionApi : ITransactionApi, ISupportRollback
{
    private readonly SugarDbContext _sugarDbContext;


    public SqlSugarTransactionApi(SugarDbContext sugarDbContext)
    {
        _sugarDbContext = sugarDbContext;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _sugarDbContext.SqlSugarClient.Ado.CommitTranAsync();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        await _sugarDbContext.SqlSugarClient.Ado.RollbackTranAsync();
    }

    public SugarDbContext GetDbContext()
    {
        return _sugarDbContext;
    }

    public void Dispose()
    {
    }
}