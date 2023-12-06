using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.Hosting;

public static class FakeHostExtensions
{
    /// <summary>
    /// host是否运行在k8s中，需要在K8s部署配置中设置"OrchestratorType"为"K8S"
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static bool IsInKubernetes(this IHost host)
    {
        var configuration = host.Services.GetRequiredService<IConfiguration>();
        var orchestratorType = configuration.GetValue<string>("OrchestratorType");
        return orchestratorType?.ToUpper() == "K8S";
    }
}