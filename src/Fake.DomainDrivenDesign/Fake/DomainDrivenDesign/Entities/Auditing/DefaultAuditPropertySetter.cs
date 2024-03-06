using Fake.Helpers;
using Fake.IdGenerators.GuidGenerator;
using Fake.Timing;
using Fake.Users;

namespace Fake.DomainDrivenDesign.Entities.Auditing;

public class DefaultAuditPropertySetter(ICurrentUser currentUser, IFakeClock fakeClock) : IAuditPropertySetter
{
    public void SetCreationProperties(object targetObject)
    {
        if (targetObject is IHasCreateTime objectWithCreationTime)
        {
            if (objectWithCreationTime.CreateTime == default)
            {
                ReflectionHelper.TrySetProperty(objectWithCreationTime, x => x.CreateTime, () => fakeClock.Now);
            }
        }

        if (targetObject is IHasCreateUserId { CreateUserId: null } objectWithCreator)
        {
            ReflectionHelper.TrySetProperty(objectWithCreator, x => x.CreateUserId, () => currentUser.Id);
        }
    }

    public void SetModificationProperties(object targetObject)
    {
        if (targetObject is IHasUpdateTime objectWithModificationTime)
        {
            ReflectionHelper.TrySetProperty(objectWithModificationTime, x => x.UpdateTime,
                () => fakeClock.Now);
        }

        if (targetObject is IHasUpdateUserId { UpdateUserId: null } objectWithModifier)
        {
            ReflectionHelper.TrySetProperty(objectWithModifier, x => x.UpdateUserId, () => currentUser.Id);
        }
    }

    public void SetVersionNumProperty(object targetObject)
    {
        if (targetObject is IHasVersionNum objectWithVersionNum)
        {
            ReflectionHelper.TrySetProperty(objectWithVersionNum, x => x.VersionNum,
                () => SimpleGuidGenerator.Instance.GenerateAsString());
        }
    }
}