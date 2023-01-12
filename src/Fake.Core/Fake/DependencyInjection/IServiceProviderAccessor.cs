namespace Fake.Core.DependencyInjection;

public interface IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }
}