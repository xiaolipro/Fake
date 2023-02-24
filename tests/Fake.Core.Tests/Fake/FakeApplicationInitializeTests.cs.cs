using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Fake;

public class FakeApplicationInitializeTests
{
    [Fact]
    void FakeApplication使用流程()
    {
        using var app = FakeApplicationFactory.Create<IndependentModuleApplication>();
        
        // Arrange
        var module = app.Services.GetSingletonInstance<IndependentModuleApplication>();
            
        // Action
        app.InitializeApplication();
        
        // Assert
        app.ServiceProvider.GetRequiredService<IndependentModuleApplication>().ShouldBeSameAs(module);
            
        // Action
        app.Shutdown();
    }


    [Fact]
    void 模块初始化前不能使用ServiceProvider()
    {
        Should.Throw<FakeException>(() =>
        {
            using var app = FakeApplicationFactory.Create<IndependentModuleApplication>();
            app.ServiceProvider.GetRequiredService<IndependentModuleApplication>();
        });
    }
}