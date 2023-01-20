using System;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Testing;

public abstract class FakeModuleTest<TStartupModule> : FakeTest, IDisposable
    where TStartupModule : IFakeModule
{
    protected IFakeApplication Application { get; set; }
    
    protected IServiceProvider RootServiceProvider { get; set; }
    
    protected IServiceScope TestServiceScope { get; }


    protected FakeModuleTest()
    {
        var services = new ServiceCollection();
        
        var application = services.AddStartupModule<TStartupModule>();
        Application = application;

        AfterAddStartupModule(services);
        RootServiceProvider = services.BuildServiceProviderFromFactory();;

        TestServiceScope = RootServiceProvider!.CreateScope();
        application.InitializeModules(TestServiceScope.ServiceProvider);

        ServiceProvider = application.ServiceProvider;
    }

    protected virtual void AfterAddStartupModule(ServiceCollection services)
    {
    }

    public void Dispose()
    {
        Application?.Shutdown();
        TestServiceScope?.Dispose();
        Application?.Dispose();
    }
}