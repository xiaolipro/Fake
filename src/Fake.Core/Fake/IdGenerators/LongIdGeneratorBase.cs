namespace Fake.IdGenerators;

public abstract class LongIdGeneratorBase : IIdGenerator<long>
{
    public abstract long Generate();
}