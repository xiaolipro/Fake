namespace Fake.Domain.Entities.Auditing;

public interface ISoftDelete
{
    /// <summary>
    /// 已经被删除
    /// </summary>
    bool IsDeleted { get; }
}