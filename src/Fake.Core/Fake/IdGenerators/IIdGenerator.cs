namespace Fake.IdGenerators;

public interface IIdGenerator<out TId> where TId : notnull
{
    TId Generate();
}