using System;
using Fake.DependencyInjection;
using Fake.Domain.Entities.Auditing;
using Fake.Identity.Users;
using Fake.Reflection;
using Fake.Timing;

namespace Fake.Auditing;

public interface IAuditPropertySetter
{
    void SetCreationProperties(object targetObject);

    void SetModificationProperties(object targetObject);

    void SetDeletionProperties(object targetObject);
}


public class AuditPropertySetter : IAuditPropertySetter, ITransientDependency
{
    private readonly ICurrentUser _currentUser;
    private readonly IClock _clock;

    public AuditPropertySetter(ICurrentUser currentUser,IClock clock)
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
        
        if (targetObject is IHasCreator<int> objectWithCreator)
        {
            if (objectWithCreator.CreatorId != default && int.TryParse(_currentUser.UserId, out var userIdAsInt))
            {
                ReflectionHelper.TrySetProperty(objectWithCreator, x => x.CreatorId, () => userIdAsInt);
            }
        }
    }

    public void SetModificationProperties(object targetObject)
    {
        throw new System.NotImplementedException();
    }

    public void SetDeletionProperties(object targetObject)
    {
        throw new System.NotImplementedException();
    }
}