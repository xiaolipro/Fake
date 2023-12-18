using Fake.Identity.Users;
using Fake.Helpers;
using Fake.Timing;

namespace Fake.DomainDrivenDesign.Entities.Auditing;

public class DefaultAuditPropertySetter : IAuditPropertySetter
{
    private readonly ICurrentUser _currentUser;
    private readonly IFakeClock _fakeClock;

    public DefaultAuditPropertySetter(ICurrentUser currentUser,IFakeClock fakeClock)
    {
        _currentUser = currentUser;
        _fakeClock = fakeClock;
    }
    public void SetCreationProperties(object targetObject)
    {
        if (targetObject is IHasCreationTime objectWithCreationTime)
        {
            if (objectWithCreationTime.CreationTime == default)
            {
                ReflectionHelper.TrySetProperty(objectWithCreationTime, x=>x.CreationTime, () => _fakeClock.Now);
            }
        }
        
        if (targetObject is IHasCreator<Guid> objectWithGuidCreator)
        {
            if (objectWithGuidCreator.CreatorId == default && Guid.TryParse(_currentUser.UserId, out var userIdAsGuid))
            {
                ReflectionHelper.TrySetProperty(objectWithGuidCreator, x => x.CreatorId, () => userIdAsGuid);
            }
        }
    }

    public void SetModificationProperties(object targetObject)
    {
        if (targetObject is IHasModificationTime objectWithModificationTime)
        {
            ReflectionHelper.TrySetProperty(objectWithModificationTime, x=>x.LastModificationTime, () => _fakeClock.Now);
        }
        
        if (targetObject is IHasModifier<Guid> objectWithGuidModifier)
        {
            if (objectWithGuidModifier.LastModifierId == default && Guid.TryParse(_currentUser.UserId, out var userIdAsGuid))
            {
                ReflectionHelper.TrySetProperty(objectWithGuidModifier, x => x.LastModifierId, () => userIdAsGuid);
            }
        }
    }
}