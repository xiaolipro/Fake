using Bang.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Bang;

public class BangApplicationInitializeTests
{
    [Fact]
    void BangApplication使用流程()
    {
        using var app = BangApplicationFactory.Create<IndependentModule>();
        
        // Arrange
        var module = app.Services.GetSingletonInstance<IndependentModule>();
            
        // Action
        app.InitializeModules();
        
        // Assert
        app.ServiceProvider.GetRequiredService<IndependentModule>().ShouldBeSameAs(module);
            
        // Action
        app.Shutdown();
    }


    [Fact]
    void 模块初始化前不能使用ServiceProvider()
    {
        Should.Throw<BangException>(() =>
        {
            using var app = BangApplicationFactory.Create<IndependentModule>();
            app.ServiceProvider.GetRequiredService<IndependentModule>();
        });
    }
}