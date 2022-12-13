namespace Fast.Core.DependencyInjection;

public enum ExposePolicy
{
    /// <summary>
    /// 仅暴露实现
    /// </summary>
    OnlyImplement,

    /// <summary>
    /// 暴露第一个接口
    /// </summary>
    FirstInterface,

    /// <summary>
    /// 暴露按命名约定的接口
    /// </summary>
    /// <remarks>
    /// 实现类的暴露接口是'I'+实现类类名，例如：A的暴露接口（服务）是IA
    /// </remarks>
    NamingConventions,

    /// <summary>
    /// 暴露实现的所有接口
    /// </summary>
    AllInterfaces
}