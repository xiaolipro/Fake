namespace Fake.Domain.Exceptions;

public class EntityNotFoundException(Type entityType, object? id = null) : DomainException
{
    public Type? EntityType { get; set; } = entityType;

    public object? Id { get; set; } = id;
}