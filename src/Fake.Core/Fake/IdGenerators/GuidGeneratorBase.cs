namespace Fake.IdGenerators;

public abstract class GuidGeneratorBase : IIdGenerator<Guid>
{
    public abstract Guid Generate();

    public virtual string GenerateAsString(string format = "N")
    {
        return Guid.NewGuid().ToString(format);
    }
}