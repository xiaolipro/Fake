using System.Threading.Tasks;

namespace Fake.UnitOfWork;


/// <summary>
/// 数据库上下文容器
/// </summary>
public interface IDbContextContainer
{
    Task<IDbContext> GetDbContextAsync();
}

public interface IDbContext
{
    
}