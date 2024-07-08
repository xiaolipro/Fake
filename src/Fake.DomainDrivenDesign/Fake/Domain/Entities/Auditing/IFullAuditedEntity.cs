namespace Fake.Domain.Entities.Auditing;

public interface IFullAuditedEntity : IEntity, IHasCreateUserId, IHasUpdateUserId
    , IHasCreateTime, IHasUpdateTime, ISoftDelete;