namespace Fake.Auditing;

/// <summary>
/// 完全审计对象
/// </summary>
/// <typeparam name="TUserKey"></typeparam>
public interface IFullAuditedObject<out TUserKey> : IHasCreator<TUserKey>, IHasModifier<TUserKey>, IHasDeleter<TUserKey>
{
    
}
