using Bang.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Bang.Modularity;

public class ModuleLoaderTests
{
    [Fact]
    void 应该按照依赖顺序加载模块()
    {
        var loader = new BangModuleLoader();
        var services = new ServiceCollection();
        services.AddSingleton<IBangLoggerFactory>(new DefaultBangLoggerFactory());
        var modules = loader.LoadModules(services, typeof(StartupModule));
        
        modules.Length.ShouldBe(2);
        modules[0].Type.ShouldBe(typeof(CustomModule));
        modules[1].Type.ShouldBe(typeof(StartupModule));
    }
    
    [Fact]
    void 没有注册日志应该抛异常()
    {
        Should.Throw<InvalidOperationException>(() =>
        {
            var loader = new BangModuleLoader();
            var services = new ServiceCollection();
            loader.LoadModules(services, typeof(StartupModule));
        });
    }
}

[DependsOn(typeof(CustomModule))]
public class StartupModule : BangModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}

public class CustomModule : BangModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}