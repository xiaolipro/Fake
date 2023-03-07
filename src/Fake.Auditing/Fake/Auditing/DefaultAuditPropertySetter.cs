using System;
using Fake.DependencyInjection;
using Fake.Domain.Entities.Auditing;
using Fake.Identity.Users;
using Fake.Reflection;
using Fake.Timing;

namespace Fake.Auditing;

public class DefaultAuditPropertySetter : IAuditPropertySetter, ITransientDependency
{
    private readonly ICurrentUser _currentUser;
    private readonly IClock _clock;

    public DefaultAuditPropertySetter(ICurrentUser currentUser,IClock clock)
    {
        _currentUser = currentUser;
        _clock = clock;
    }
    public void SetCreationProperties(object targetObject)
    {
        if (targetObject is IHasCreationTime objectWithCreationTime)
        {
            if (objectWithCreationTime.CreationTime != default)
            {
                ReflectionHelper.TrySetProperty(objectWithCreationTime, x=>x.CreationTime, () => _clock.Now);
            }
        }
        
        if (targetObject is IHasCreator<Guid> objectWithGuidCreator)
        {
            if (objectWithGuidCreator.CreatorId != default && Guid.TryParse(_currentUser.UserId, out var userIdAsGuid))
            {
                ReflectionHelper.TrySetProperty(objectWithGuidCreator, x => x.CreatorId, () => userIdAsGuid);
            }
        }
    }

    public void SetModificationProperties(object targetObject)
    {
        if (targetObject is IHasModificationTime { LastModificationTime: { } } objectWithModificationTime)
        {
            ReflectionHelper.TrySetProperty(objectWithModificationTime, x=>x.LastModificationTime, () => _clock.Now);
        }
        
        if (targetObject is IHasModifier<Guid> objectWithGuidModifier)
        {
            if (objectWithGuidModifier.LastModifierId != default && Guid.TryParse(_currentUser.UserId, out var userIdAsGuid))
            {
                ReflectionHelper.TrySetProperty(objectWithGuidModifier, x => x.LastModifierId, () => userIdAsGuid);
            }
        }
    }
}