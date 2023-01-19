namespace Fake.Auditing;

public interface IHasModifier<out TCreatorId>
{
    /// <summary>
    /// 上一次修改者Id
    /// </summary>
    TCreatorId? ModifierId { get; }
    
    /// <summary>
    /// 上一次修改时间
    /// </summary>
    DateTime? ModifyTime { get; }
}