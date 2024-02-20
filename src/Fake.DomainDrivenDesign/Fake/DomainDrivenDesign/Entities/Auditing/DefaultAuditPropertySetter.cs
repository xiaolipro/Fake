using Fake.Helpers;
using Fake.IdGenerators.GuidGenerator;
using Fake.Timing;
using Fake.Users;

namespace Fake.DomainDrivenDesign.Entities.Auditing;

public class DefaultAuditPropertySetter(ICurrentUser currentUser, IFakeClock fakeClock) : IAuditPropertySetter
{
    public void SetCreationProperties(object targetObject)
    {
        if (targetObject is IHasCreationTime objectWithCreationTime)
        {
            if (objectWithCreationTime.CreationTime == default)
            {
                ReflectionHelper.TrySetProperty(objectWithCreationTime, x => x.CreationTime, () => fakeClock.Now);
            }
        }

        if (targetObject is IHasCreator<Guid> objectWithGuidCreator)
        {
            if (objectWithGuidCreator.CreatorId == default && Guid.TryParse(currentUser.UserId, out var userIdAsGuid))
            {
                ReflectionHelper.TrySetProperty(objectWithGuidCreator, x => x.CreatorId, () => userIdAsGuid);
            }
        }
    }

    public void SetModificationProperties(object targetObject)
    {
        if (targetObject is IHasModificationTime objectWithModificationTime)
        {
            ReflectionHelper.TrySetProperty(objectWithModificationTime, x => x.LastModificationTime,
                () => fakeClock.Now);
        }

        if (targetObject is IHasModifier<Guid> objectWithGuidModifier)
        {
            if (objectWithGuidModifier.LastModifierId == default &&
                Guid.TryParse(currentUser.UserId, out var userIdAsGuid))
            {
                ReflectionHelper.TrySetProperty(objectWithGuidModifier, x => x.LastModifierId, () => userIdAsGuid);
            }
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