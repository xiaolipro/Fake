using Fake.Domain.Entities.Events;

namespace Fake.Domain.Entities;

public interface IHasDomainEvent
{
    public IReadOnlyCollection<DomainEvent> DomainEvents { get; }
}