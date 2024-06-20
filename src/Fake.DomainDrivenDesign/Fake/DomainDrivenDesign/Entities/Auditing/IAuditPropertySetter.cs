namespace Fake.DomainDrivenDesign.Entities.Auditing;

public interface IAuditPropertySetter
{
    void SetCreationProperties(IEntity entity);

    void SetModificationProperties(IEntity entity);

    void SetSoftDeleteProperty(IEntity entity);
}