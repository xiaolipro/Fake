namespace Fake.DomainDrivenDesign.Entities.Auditing;

public interface IAuditPropertySetter
{
    void SetCreationProperties(object targetObject);

    void SetModificationProperties(object targetObject);
}