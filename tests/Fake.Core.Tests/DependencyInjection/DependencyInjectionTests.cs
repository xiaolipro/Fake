using Fake.Core.Tests.Modularity;
using Fake.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Core.Tests.DependencyInjection;

public class DependencyInjectionTests
{
    [Fact]
    public void 默认会注册自己和按命名约定的接口()
    {
        using var application = FakeApplicationFactory.Create<IndependentModule>();
        application.InitializeApplication();
        var a = application.ServiceProvider.GetService<IA>();
        a.ShouldNotBeNull();

        var c = application.ServiceProvider.GetService<MyA>();
        c.ShouldNotBeNull();
    }

    [Fact]
    public void ExposeServices强行暴露没有按命名约定的接口()
    {
        using var application = FakeApplicationFactory.Create<IndependentModule>();
        application.InitializeApplication();
        application.ServiceProvider.GetService<IB>().ShouldNotBeNull();
    }

    [Fact]
    void Dependency指定的生命周期优先级最高()
    {
        using var application = FakeApplicationFactory.Create<IndependentModule>();
        application.InitializeApplication();

        application.Services.First(x => x.ServiceType == typeof(IA)).Lifetime.ShouldBe(ServiceLifetime.Transient);
    }

    [Fact]
    void DisableServiceRegistration禁用服务注册()
    {
        using var application = FakeApplicationFactory.Create<IndependentModule>();
        application.InitializeApplication();

        application.ServiceProvider.GetService<MyB>().ShouldBeNull();
    }


    [Fact]
    void ScopedSingleton的层次体系重定向()
    {
        using var application = FakeApplicationFactory.Create<IndependentModule>();
        application.InitializeApplication();

        var a = application.ServiceProvider.GetService<IHierarchy>();
        //a.GetHashCode().ShouldBe(b.GetHashCode());
        application.Services.First(x => x.ServiceType == typeof(IHierarchy))
            .ImplementationFactory
            ?.Invoke(application.ServiceProvider).GetType().ShouldBe(typeof(BHierarchy));

        application.ServiceProvider.GetServices<IHierarchy>().Count().ShouldBe(1);
    }

    [Fact]
    void Singleton大于Scoped大于Transient()
    {
        using var application = FakeApplicationFactory.Create<IndependentModule>();
        application.InitializeApplication();

        application.Services.First(x => x.ServiceType == typeof(IDifferentLife)).Lifetime
            .ShouldBe(ServiceLifetime.Singleton);
    }
}

public interface IHierarchy : ISingletonDependency;

public class AHierarchy : IHierarchy;

[Dependency(Replace = true)]
public class BHierarchy : AHierarchy;

[Dependency(ServiceLifetime.Transient)]
public class MyA : IA;

public interface IA : ISingletonDependency;

public interface IB;

[ExposeServices(typeof(IB))]
public class X : IB, IScopedDependency;

[DisableServiceRegistration]
public class MyB : IA;

public interface IDifferentLife : ISingletonDependency;

public class DifferentLife : IDifferentLife, ITransientDependency;