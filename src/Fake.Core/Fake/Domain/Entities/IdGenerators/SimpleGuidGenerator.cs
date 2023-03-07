namespace Fake.Domain.Entities.IdGenerators;

public class SimpleGuidGenerator:IGuidGenerator
{
    public static SimpleGuidGenerator Instance { get; } = new SimpleGuidGenerator();
    public Guid Create()
    {
        return Guid.NewGuid();
    }
}