using JetBrains.Annotations;

namespace Fake.Domain.Entities.Auditing;

public interface IHasModifier<out TUserId>
{
    /// <summary>
    /// 最近一次修改者Id
    /// </summary>
    [NotNull]
    TUserId LastModifierId { get; }
}