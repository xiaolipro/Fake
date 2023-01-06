
namespace Bang.Modularity;

public interface IBangModule: IModuleLifecycle
{
    public bool IsBangFrameworkModule { get; }
    public bool SkipAutoServiceRegistration { get; }
}