namespace Fake.DependencyInjection;

public class ServiceRegistrationActionList : List<Action<OnServiceRegistrationContext>>
{
    /// <summary>
    /// 禁用对类的拦截器
    /// </summary>
    public bool DisableClassInterceptors { get; set; }

    /// <summary>
    /// 属性注入覆盖原有值
    /// </summary>
    public bool PropertyInjectionCover { get; set; } = true;
}