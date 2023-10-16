namespace Fake.Modularity;

public interface IFakeModule : IConfigureServicesLifecycle, IConfigureApplicationLifecycle, IShutdownLifecycle
{
}