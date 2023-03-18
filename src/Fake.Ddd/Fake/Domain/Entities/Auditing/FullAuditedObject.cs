#nullable enable
namespace Fake.Domain.Entities.Auditing;

/// <summary>
/// 完全审计对象
/// </summary>
/// <typeparam name="TUserId">用户id类型</typeparam>
public class FullAuditedObject<TUserId> : IHasCreator<TUserId>, IHasModifier<TUserId>
    , IHasCreationTime, IHasModificationTime, ISoftDelete
{
    public TUserId CreatorId { get; }
    public DateTime CreationTime { get; }
    public TUserId? LastModifierId { get; }
    public DateTime? LastModificationTime { get; }
    public bool IsDeleted { get; }
    public bool HardDeleted { get; }
}