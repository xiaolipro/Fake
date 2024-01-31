using Fake.Collections;
using Fake.DynamicProxy;

namespace Fake.DependencyInjection;

public class OnServiceRegistrationContext(Type serviceType, Type implementationType)
{
    /// <summary>
    /// 服务拦截器
    /// </summary>
    public virtual ITypeList<IFakeInterceptor> Interceptors { get; } = new TypeList<IFakeInterceptor>();

    /// <summary>
    /// 服务类型
    /// </summary>
    public virtual Type ServiceType { get; } = ThrowHelper.ThrowIfNull(serviceType, nameof(serviceType));

    /// <summary>
    /// 实现类型
    /// </summary>
    public virtual Type ImplementationType { get; } =
        ThrowHelper.ThrowIfNull(implementationType, nameof(implementationType));
}