namespace Fake.DependencyInjection;

public class OnServiceExposingContext
{
    public Type ImplementationType { get; }

    public List<ServiceIdentifier> ExposedServices { get; }

    public OnServiceExposingContext(Type implementationType, List<ServiceIdentifier> exposedServices)
    {
        ImplementationType = ThrowHelper.ThrowIfNull(implementationType, nameof(implementationType));
        ExposedServices = ThrowHelper.ThrowIfNull(exposedServices, nameof(exposedServices));
    }
}