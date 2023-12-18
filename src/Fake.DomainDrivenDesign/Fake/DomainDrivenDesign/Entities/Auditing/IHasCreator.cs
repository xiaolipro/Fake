#nullable enable
namespace Fake.DomainDrivenDesign.Entities.Auditing;

public interface IHasCreator<out TUserId>
{
    /// <summary>
    /// 创建者Id
    /// </summary>
    TUserId? CreatorId { get; }
}