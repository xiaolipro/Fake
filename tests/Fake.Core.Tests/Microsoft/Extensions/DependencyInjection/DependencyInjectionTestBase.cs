using Fake.DependencyInjection;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Core.Tests.Microsoft.Extensions.DependencyInjection;

public abstract class DependencyInjectionTestBase : ApplicationTest<DependencyInjectTestModule>
{
    [Fact]
    public void 单例服务可以跨越瞬态scope()
    {
        MySingletonService singletonService;
        MyTransientService transientService;
        MyEmptyTransientService emptyTransientService;

        using (var scope = ServiceProvider.CreateScope())
        {
            transientService = scope.ServiceProvider.GetRequiredService<MyTransientService>();
            emptyTransientService = scope.ServiceProvider.GetRequiredService<MyEmptyTransientService>();

            transientService.DoIt();
            transientService.DoIt();

            // 此时瞬态生命周期还没结束
            transientService.SingletonService.TransientInstances.Count.ShouldBe(2);
            transientService.TransientInstances.Count.ShouldBe(2);
            transientService.TransientInstances.ForEach(s => s.IsDisposed.ShouldBeFalse());

            singletonService = transientService.SingletonService;
        }

        Assert.Equal(singletonService, ServiceProvider.GetRequiredService<MySingletonService>());

        singletonService.ResolveTransient();

        singletonService.TransientInstances.Count.ShouldBe(3);
        singletonService.TransientInstances.ForEach(s => s.IsDisposed.ShouldBeFalse());
        transientService.TransientInstances.ForEach(s => s.IsDisposed.ShouldBeTrue());

        emptyTransientService.IsDisposed.ShouldBeTrue();
    }

    [Fact]
    public void 属性注入必须public且具有set()
    {
        ServiceProvider.GetRequiredService<ServiceWithPropertyInject>().PropertyInjectedServiceWithPublicSet
            .ShouldNotBeNull();
        ServiceProvider.GetRequiredService<ServiceWithPropertyInject>().PropertyInjectedServiceWithPrivateSet
            .ShouldBeNull();
        ServiceProvider.GetRequiredService<ServiceWithPropertyInject>().PropertyInjectedServiceWithProtectedSet
            .ShouldBeNull();
        ServiceProvider.GetRequiredService<ServiceWithPropertyInject>().PropertyInjectedServiceWithInternalSet
            .ShouldBeNull();
        ServiceWithPropertyInject.StaticPropertyInjectedServiceWithPublicSet.ShouldBeNull();
    }

    public class ServiceWithPropertyInject : ITransientDependency
    {
        public MyEmptyTransientService PropertyInjectedServiceWithPublicSet { get; set; } = null!;

        public MyEmptyTransientService PropertyInjectedServiceWithPrivateSet { get; private set; } = null!;

        public MyEmptyTransientService PropertyInjectedServiceWithProtectedSet { get; protected set; } = null!;
        public MyEmptyTransientService PropertyInjectedServiceWithInternalSet { get; internal set; } = null!;

        public static MyEmptyTransientService StaticPropertyInjectedServiceWithPublicSet { get; set; } = null!;
    }


    public class MySingletonService : ISingletonDependency
    {
        public List<MyEmptyTransientService> TransientInstances { get; }

        public IServiceProvider ServiceProvider { get; }

        public MySingletonService(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            TransientInstances = new List<MyEmptyTransientService>();
        }

        public void ResolveTransient()
        {
            TransientInstances.Add(
                ServiceProvider.GetRequiredService<MyEmptyTransientService>()
            );
        }
    }

    public class MyTransientService : ITransientDependency
    {
        public MySingletonService SingletonService { get; }
        public IServiceProvider ServiceProvider { get; }
        public List<MyEmptyTransientService> TransientInstances { get; }

        public MyTransientService(MySingletonService singletonService, IServiceProvider serviceProvider)
        {
            SingletonService = singletonService;
            ServiceProvider = serviceProvider;
            TransientInstances = new List<MyEmptyTransientService>();
        }

        public void DoIt()
        {
            SingletonService.ResolveTransient();

            TransientInstances.Add(
                ServiceProvider.GetRequiredService<MyEmptyTransientService>()
            );
        }
    }

    public class MyEmptyTransientService : ITransientDependency, IDisposable
    {
        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}