
namespace Fake.Modularity;

public interface IFakeModule: IModuleLifecycle
{
    public bool IsFakeFrameworkModule { get; }
    public bool SkipAutoServiceRegistration { get; }
}