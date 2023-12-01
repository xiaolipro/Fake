namespace Fake.DependencyInjection;

public class OnServiceExposingContext
{
    public Type ImplementationType { get; }

    public List<Type> ExposedServiceTypes { get; }

    public OnServiceExposingContext(Type implementationType, List<Type> exposedServiceTypes)
    {
        ImplementationType = ThrowHelper.ThrowIfNull(implementationType, nameof(implementationType));
        ExposedServiceTypes = ThrowHelper.ThrowIfNull(exposedServiceTypes, nameof(exposedServiceTypes));
    }
}