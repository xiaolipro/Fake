using System.Threading;
using System.Threading.Tasks;

namespace Fake.Domain.Repositories.EntityFrameWorkCore;

/// <summary>
/// 无根仓储，面向数据库上下文操作
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
public interface IEfCoreNoRootRepository<TDbContext>
{
    Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default);
}