#nullable enable
namespace Fake.DomainDrivenDesign.Entities.Auditing;

/// <summary>
/// 完全审计实体
/// </summary>
/// <typeparam name="TKey">id类型</typeparam>
/// <typeparam name="TUserId">用户id类型</typeparam>
public class FullAuditedEntity<TKey, TUserId> : Entity<TKey>, IFullAuditedEntity<TUserId>
{
    public TUserId? CreatorId { get; set; }
    public DateTime CreationTime { get; set; }
    public TUserId? LastModifierId { get; set; }
    public DateTime? LastModificationTime { get; set; }
    public bool IsDeleted { get; set; }
    public bool HardDeleted { get; set; }
}

public interface IFullAuditedEntity<out TUserId> : IHasCreator<TUserId>, IHasModifier<TUserId>
    , IHasCreationTime, IHasModificationTime, ISoftDelete
{
}