using JetBrains.Annotations;

namespace Fake.Domain.Entities.Auditing;

public interface IHasCreator<out TUserId>
{
    /// <summary>
    /// 创建者Id
    /// </summary>
    [CanBeNull]
    TUserId CreatorId { get; }
}