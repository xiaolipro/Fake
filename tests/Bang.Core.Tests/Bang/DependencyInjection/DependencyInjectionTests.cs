using Bang.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Bang.DependencyInjection;

public class DependencyInjectionTests
{
    [Fact]
    public void 默认会注册自己和按命名约定的接口()
    {
        using var application = BangApplicationFactory.Create<IndependentModule>();
        application.InitializeModules();
        var a = application.ServiceProvider.GetService<IA>();
        a.ShouldNotBeNull();

        var c = application.ServiceProvider.GetService<MyA>();
        c.ShouldNotBeNull();
    }

    [Fact]
    public void ExposeServices强行暴露没有按命名约定的接口()
    {
        using (var application = BangApplicationFactory.Create<IndependentModule>())
        {
            application.InitializeModules();

            application.ServiceProvider.GetService<IB>().ShouldNotBeNull();
        }
    }

    [Fact]
    void Dependency指定的生命周期优先级最高()
    {
        using var application = BangApplicationFactory.Create<IndependentModule>();
        application.InitializeModules();

        application.Services.First(x => x.ServiceType == typeof(IA)).Lifetime.ShouldBe(ServiceLifetime.Transient);
    }

    [Fact]
    void DisableServiceRegistration禁用服务注册()
    {
        using var application = BangApplicationFactory.Create<IndependentModule>();
        application.InitializeModules();

        application.ServiceProvider.GetService<MyB>().ShouldBeNull();
    }


    [Fact]
    void ScopedSingleton的层次体系重定向()
    {
        using var application = BangApplicationFactory.Create<IndependentModule>();
        application.InitializeModules();

        var a = application.ServiceProvider.GetService<IHierarchy>();
        var b = application.ServiceProvider.GetService<AHierarchy>();
        //a.GetHashCode().ShouldBe(b.GetHashCode());
        application.Services.First(x => x.ServiceType == typeof(IHierarchy)).ImplementationFactory
            .Invoke(application.ServiceProvider).GetType().ShouldBe(typeof(BHierarchy));
        
        application.ServiceProvider.GetServices<AHierarchy>().Count().ShouldBe(1);
    }
}

public interface IHierarchy : ISingletonDependency
{
}

public class AHierarchy : IHierarchy
{
}

[Dependency(Replace = true)]
public class BHierarchy : AHierarchy
{
}

[Dependency(ServiceLifetime.Transient)]
public class MyA : IA
{
}

public interface IA : ISingletonDependency
{
}

public interface IB
{
}

[ExposeServices(typeof(IB))]
public class X : IB, IScopedDependency
{
}

[DisableServiceRegistration]
public class MyB : IA
{
}