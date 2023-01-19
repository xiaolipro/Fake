namespace Fake.Auditing;

public interface IHasCreator<out TCreatorId>
{
    /// <summary>
    /// 创建者Id
    /// </summary>
    TCreatorId? CreatorId { get; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    DateTime? CreationTime { get; }
}