using Fake.Modularity;

namespace Fake;

public interface IFakeApplicationInfo:IModuleContainer,IApplicationInfo,IDisposable
{
    /// <summary>
    /// 启动模块的类型
    /// </summary>
    Type StartupModuleType { get; }
    
    /// <summary>
    /// 服务容器。应用程序初始化后，无法将新服务添加到此容器。
    /// </summary>
    IServiceCollection Services { get; }
    
    /// <summary>
    /// 服务供应商。模块初始化前，不能使用。
    /// </summary>
    IServiceProvider ServiceProvider { get; }
    
    /// <summary>
    /// 调用模块的Pre/Post/ConfigureServicesAsync方法
    /// </summary>
    void ConfigureServices();
    
    /// <summary>
    /// 程序关闭
    /// </summary>
    void Shutdown();
}