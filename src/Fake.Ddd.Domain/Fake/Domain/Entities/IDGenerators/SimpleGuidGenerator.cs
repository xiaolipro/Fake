namespace Fake.Domain.Entities.IDGenerators;

public class SimpleGuidGenerator:IGuidGenerator
{
    public static SimpleGuidGenerator Instance { get; } = new SimpleGuidGenerator();
    public Guid Generate()
    {
        return Guid.NewGuid();
    }
}