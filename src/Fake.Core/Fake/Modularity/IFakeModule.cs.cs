
namespace Fake.Modularity;

public interface IFakeModuleApplication: IModuleApplicationLifecycle
{
    public bool IsFakeFrameworkModule { get; }
    public bool SkipAutoServiceRegistration { get; }
}