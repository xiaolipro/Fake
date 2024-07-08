using Fake.Data.Filtering;

namespace Fake.Domain.Entities.Auditing;

public interface ISoftDelete : ICanFilter
{
    /// <summary>
    /// 已经删除
    /// </summary>
    bool IsDeleted { get; }
}