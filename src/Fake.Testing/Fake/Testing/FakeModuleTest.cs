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
        var services = CreateServiceCollection();
        
        BeforeAddStartupModule(services);
        var application = services.AddStartupModule<TStartupModule>(SetFakeApplicationCreationOptions);
        AfterAddStartupModule(services);
        
        Application = application;

        RootServiceProvider = services.BuildServiceProviderFromFactory();;

        TestServiceScope = RootServiceProvider!.CreateScope();
        application.InitializeApplication(TestServiceScope.ServiceProvider);

        ServiceProvider = application.ServiceProvider;
    }
    
    protected virtual IServiceCollection CreateServiceCollection()
    {
        return new ServiceCollection();
    }

    protected virtual void BeforeAddStartupModule(IServiceCollection services)
    {

    }
    
    protected virtual void AfterAddStartupModule(IServiceCollection services)
    {
    }
    
    protected virtual void SetFakeApplicationCreationOptions(FakeApplicationCreationOptions options)
    {

    }
    
    protected virtual IServiceProvider CreateServiceProvider(IServiceCollection services)
    {
        return services.BuildServiceProviderFromFactory();
    }

    public void Dispose()
    {
        Application?.Shutdown();
        TestServiceScope?.Dispose();
        Application?.Dispose();
    }
}