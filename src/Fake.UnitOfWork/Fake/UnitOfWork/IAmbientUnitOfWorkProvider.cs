namespace Fake.UnitOfWork;

public interface IAmbientUnitOfWorkProvider : IUnitOfWorkAccessor
{
    IUnitOfWork? GetCurrentByChecking();
}