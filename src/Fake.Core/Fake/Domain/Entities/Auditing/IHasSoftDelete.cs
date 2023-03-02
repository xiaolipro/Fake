namespace Fake.Domain.Entities.Auditing;

public interface IHasSoftDelete
{
    /// <summary>
    /// 已经被删除
    /// </summary>
    bool IsDeleted { get; }
}