using System;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Testing;

public abstract class FakeModuleTestBase<TStartupModule> : FakeTestBase, IDisposable
    where TStartupModule : IFakeModule
{
    protected IFakeApplication Application { get; set; }
    
    protected IServiceProvider RootServiceProvider { get; set; }
    
    protected IServiceScope TestServiceScope { get; }


    protected FakeModuleTestBase()
    {
        var services = CreateServiceCollection();
        
        BeforeAddFakeApplication(services);
        var application = services.AddFakeApplication<TStartupModule>(SetApplicationCreationOptions);
        AfterAddFakeApplication(services);
        
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

    protected virtual void BeforeAddFakeApplication(IServiceCollection services)
    {

    }
    
    protected virtual void AfterAddFakeApplication(IServiceCollection services)
    {
    }
    
    protected virtual void SetApplicationCreationOptions(FakeApplicationCreationOptions options)
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