#nullable enable
namespace Fake.Domain.Entities.Auditing;

/// <summary>
/// 完全审计对象
/// </summary>
/// <typeparam name="TUserKey">用户id类型</typeparam>
public class FullAuditedObject<TUserKey> : IHasCreator<TUserKey>, IHasModifier<TUserKey>, IHasDeleter<TUserKey>
{
    public TUserKey CreatorId { get; }
    public DateTime CreationTime { get; }
    public TUserKey LastModifierId { get; }
    public DateTime LastModificationTime { get; }
    public TUserKey? DeleterId { get; }
    public DateTime? DeletionTime { get; }
}