using System.Threading.Tasks;

namespace Fake.Authorization;

public interface IMethodAuthorizationService
{
    /// <summary>
    /// 检验是否有权限访问
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task CheckAsync(MethodAuthorizationContext context);
}