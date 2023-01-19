namespace Fake.Auditing;

public interface IHasDeleter<out TCreatorId>
{
    /// <summary>
    /// 删除者Id
    /// </summary>
    TCreatorId? DeleterId { get; }
    
    /// <summary>
    /// 删除时间
    /// </summary>
    DateTime? DeletionTime { get; }
}