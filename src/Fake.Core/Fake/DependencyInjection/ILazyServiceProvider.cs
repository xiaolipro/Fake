namespace Fake.DependencyInjection;

public interface ILazyServiceProvider
{
    T GetLazyService<T>(Func<IServiceProvider, object> valueFactory = null) where T: class;

    object GetLazyService(Type serviceType, Func<IServiceProvider, object> valueFactory = null);

    T GetRequiredLazyService<T>() where T: class;

    object GetRequiredLazyService(Type serviceType);
}