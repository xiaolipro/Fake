namespace Fake.DependencyInjection;

public interface IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }
}