using Fake.Data.Filtering;

namespace Fake.DomainDrivenDesign.Entities.Auditing;

public interface ISoftDelete : ICanDataFilter
{
    /// <summary>
    /// 已经删除
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    /// 硬删除，物理删除
    /// </summary>
    bool HardDeleted { get; }
}