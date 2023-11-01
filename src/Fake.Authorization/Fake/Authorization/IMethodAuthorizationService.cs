using System.Reflection;
using System.Threading.Tasks;

namespace Fake.Authorization;

public interface IMethodAuthorizationService
{
    Task<bool> IsGrantedAsync(MethodInfo invocationMethod);
}