using Fake.Helpers;
using Fake.Timing;
using Fake.Users;

namespace Fake.Domain.Entities.Auditing;

public class DefaultAuditPropertySetter(ICurrentUser currentUser, IFakeClock fakeClock) : IAuditPropertySetter
{
    public void SetCreationProperties(IEntity entity)
    {
        if (entity is IHasCreateTime entityWithCreationTime)
        {
            if (entityWithCreationTime.CreateTime == default)
            {
                ReflectionHelper.TrySetProperty(entityWithCreationTime, x => x.CreateTime, () => fakeClock.Now);
            }
        }

        if (entity is IHasCreateUserId entityWithCreateUserId && entityWithCreateUserId.CreateUserId == default)
        {
            ReflectionHelper.TrySetProperty(entityWithCreateUserId, x => x.CreateUserId, () => currentUser.Id);
        }
    }

    public void SetModificationProperties(IEntity entity)
    {
        if (entity is IHasUpdateTime entityWithModificationTime)
        {
            ReflectionHelper.TrySetProperty(entityWithModificationTime, x => x.UpdateTime,
                () => fakeClock.Now);
        }

        if (entity is IHasUpdateUserId entityWithUpdateUserId && entityWithUpdateUserId.UpdateUserId == default)
        {
            ReflectionHelper.TrySetProperty(entityWithUpdateUserId, x => x.UpdateUserId, () => currentUser.Id);
        }
    }

    public void SetSoftDeleteProperty(IEntity entity)
    {
        if (entity is not ISoftDelete entityWithSoftDelete) return;

        ReflectionHelper.TrySetProperty(entityWithSoftDelete, x => x.IsDeleted, () => true);
    }
}