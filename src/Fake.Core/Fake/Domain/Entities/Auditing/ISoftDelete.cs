namespace Fake.Domain.Entities.Auditing;

public interface ISoftDelete
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