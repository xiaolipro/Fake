using System.Security.Claims;
using Fake.DependencyInjection;
using Fake.Testing;

namespace Microsoft.Extensions.DependencyInjection;

public abstract class AbstractDependencyInjectionTests:FakeIntegrationTest<DependencyInjectTestModule>
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
        
        Assert.Equal(singletonService, GetRequiredService<MySingletonService>());
        
        singletonService.ResolveTransient();
        
        singletonService.TransientInstances.Count.ShouldBe(3);
        singletonService.TransientInstances.ForEach(s => s.IsDisposed.ShouldBeFalse());
        transientService.TransientInstances.ForEach(s => s.IsDisposed.ShouldBeTrue());
        
        emptyTransientService.IsDisposed.ShouldBeTrue();
    }

    [Fact]
    public void 支持属性注入()
    {
        GetRequiredService<ServiceWithPropertyInject>().PropertyInjectedServiceWithPublicSet.ShouldNotBeNull();
        GetRequiredService<ServiceWithPropertyInject>().PropertyInjectedServiceWithPrivateSet.ShouldBeNull();
        GetRequiredService<ServiceWithPropertyInject>().PropertyInjectedServiceWithProtectedSet.ShouldBeNull();
        GetRequiredService<ServiceWithPropertyInject>().PropertyInjectedServiceWithInternalSet.ShouldBeNull();
        ServiceWithPropertyInject.StaticPropertyInjectedServiceWithPublicSet.ShouldBeNull();
    }

    public class ServiceWithPropertyInject : ITransientDependency
    {
        public MyEmptyTransientService PropertyInjectedServiceWithPublicSet { get; set; }
        
        public MyEmptyTransientService PropertyInjectedServiceWithPrivateSet { get;private set; }
        
        public MyEmptyTransientService PropertyInjectedServiceWithProtectedSet { get;protected set; }
        public MyEmptyTransientService PropertyInjectedServiceWithInternalSet { get;internal set; }
        
        public static MyEmptyTransientService StaticPropertyInjectedServiceWithPublicSet { get; set; }
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