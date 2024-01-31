using System.Threading.Tasks;

namespace Fake.Authorization.Permissions;

public interface IPermissionChecker
{
    Task<bool> IsGrantedAsync(params string[] permissions);
    Task<bool> IsGrantedAsync(PermissionRequirement requirement);
}