namespace Fake.Domain.Entities.Auditing;

public interface IAuditPropertySetter
{
    void SetCreationProperties(object targetObject);

    void SetModificationProperties(object targetObject);
}