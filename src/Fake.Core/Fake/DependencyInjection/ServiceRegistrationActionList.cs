namespace Fake.DependencyInjection;

public class ServiceRegistrationActionList: List<Action<OnServiceRegistrationContext>>
{
    /// <summary>
    /// 禁用类的拦截器
    /// </summary>
    public bool DisableClassInterceptors { get; set; }
}