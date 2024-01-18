namespace Fake.IdGenerators.GuidGenerator;

public class SimpleGuidGenerator : GuidGeneratorBase
{
    public static SimpleGuidGenerator Instance { get; } = new();

    public override Guid Generate()
    {
        return Guid.NewGuid();
    }
}