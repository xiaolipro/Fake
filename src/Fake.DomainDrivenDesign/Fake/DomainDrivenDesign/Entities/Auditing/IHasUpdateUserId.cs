namespace Fake.DomainDrivenDesign.Entities.Auditing;

public interface IHasUpdateUserId
{
    /// <summary>
    /// 更新用户Id
    /// </summary>
    Guid? UpdateUserId { get; }
}