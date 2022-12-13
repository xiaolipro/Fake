namespace Fast.Core.DependencyInjection;

public class ExposeServicesAttribute : Attribute
{
    /// <summary>
    /// 暴露的所有服务
    /// </summary>
    public Type[] ServiceTypes { get; set; }

    /// <summary>
    /// 暴露策略
    /// </summary>
    public ExposePolicy Policy { get; set; }

    public ExposeServicesAttribute(params Type[] serviceTypes)
    {
        ServiceTypes = serviceTypes;
    }
}