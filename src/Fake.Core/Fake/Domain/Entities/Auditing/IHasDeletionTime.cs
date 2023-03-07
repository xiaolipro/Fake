namespace Fake.Domain.Entities.Auditing;

public interface IHasDeletionTime : ISoftDelete
{
    /// <summary>
    /// 删除时间
    /// </summary>
    DateTime? DeletionTime { get; }
}