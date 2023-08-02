namespace Fake.Modularity;

public interface IFakeModule : IConfigureServicesLifecycle, IConfigureApplicationLifecycle, IShutdownLifecycle
{
    /// <summary>
    /// 当赋值为true时 跳过Fake提供的自动服务注册
    /// </summary>
    public bool SkipServiceRegistration { get; }
}