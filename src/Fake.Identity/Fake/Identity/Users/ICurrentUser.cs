using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;

namespace Fake.Identity.Users;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }

    /// <summary>
    /// 用户唯一标识
    /// </summary>
    public string UserId { get; }
    
    public string UserName { get; }
}