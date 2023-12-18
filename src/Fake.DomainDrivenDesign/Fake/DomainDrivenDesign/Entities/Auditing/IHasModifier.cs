#nullable enable

namespace Fake.DomainDrivenDesign.Entities.Auditing;

public interface IHasModifier<out TUserId>
{
    /// <summary>
    /// 最近一次修改者Id
    /// </summary>
    TUserId? LastModifierId { get; }
}