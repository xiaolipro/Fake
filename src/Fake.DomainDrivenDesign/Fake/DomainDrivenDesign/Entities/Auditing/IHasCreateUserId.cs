namespace Fake.DomainDrivenDesign.Entities.Auditing;

public interface IHasCreateUserId
{
    /// <summary>
    /// 创建用户Id
    /// </summary>
    Guid CreateUserId { get; }
}