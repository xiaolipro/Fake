namespace Fake.Domain.Entities.IdGenerators;

public class SimpleGuidGenerator:IGuidGenerator
{
    public Guid Create()
    {
        return Guid.NewGuid();
    }
}