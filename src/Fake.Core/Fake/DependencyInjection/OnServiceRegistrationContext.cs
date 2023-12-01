using Fake.Collections;
using Fake.DynamicProxy;

namespace Fake.DependencyInjection;

public class OnServiceRegistrationContext
{
    /// <summary>
    /// 服务拦截器
    /// </summary>
    public virtual ITypeList<IFakeInterceptor> Interceptors { get; }

    public virtual Type ServiceType { get; }

    public virtual Type ImplementationType { get; }

    public OnServiceRegistrationContext(Type serviceType, Type implementationType)
    {
        ServiceType = ThrowHelper.ThrowIfNull(serviceType, nameof(serviceType));
        ImplementationType = ThrowHelper.ThrowIfNull(implementationType, nameof(implementationType));

        Interceptors = new TypeList<IFakeInterceptor>();
    }
}