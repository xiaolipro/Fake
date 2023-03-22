#nullable enable
namespace Fake.Domain.Entities.Auditing;

/// <summary>
/// 完全审计实体
/// </summary>
/// <typeparam name="TUserId">用户id类型</typeparam>
public class FullAuditedEntity<TUserId> : IFullAuditedEntity<TUserId>
{
    public TUserId CreatorId { get; set; }
    public DateTime CreationTime { get; set; }
    public TUserId LastModifierId { get; set; }
    public DateTime LastModificationTime { get; set; }
    public bool IsDeleted { get; set; }
    public bool HardDeleted { get; set; }
}

public interface IFullAuditedEntity<out TUserId> : IHasCreator<TUserId>, IHasModifier<TUserId>
    , IHasCreationTime, IHasModificationTime, ISoftDelete
{
}