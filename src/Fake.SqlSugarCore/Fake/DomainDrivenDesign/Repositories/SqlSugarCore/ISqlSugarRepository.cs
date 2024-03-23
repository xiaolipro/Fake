using System.Threading.Tasks;
using Fake.DomainDrivenDesign.Entities;
using SqlSugar;

namespace Fake.DomainDrivenDesign.Repositories.SqlSugarCore;

public interface ISqlSugarRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IAggregateRoot
{
    Task<ISqlSugarClient> GetDbContextAsync();
}