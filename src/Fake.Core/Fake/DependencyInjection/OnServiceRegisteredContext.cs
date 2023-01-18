using Fake.Proxy;

namespace Fake.DependencyInjection;

public class OnServiceRegisteredContext
{
    public virtual List<IFakeInterceptor> Interceptors { get; }
    
    public virtual Type ServiceType { get; }
    
    public virtual Type ImplementationType { get; }
    
    public OnServiceRegisteredContext([NotNull]Type serviceType, [NotNull] Type implementationType)
    {
        ServiceType = ThrowHelper.ThrowIfNull(serviceType, nameof(serviceType));
        ImplementationType = ThrowHelper.ThrowIfNull(implementationType, nameof(implementationType));

        Interceptors = new List<IFakeInterceptor>();
    }
}