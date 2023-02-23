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
    [CanBeNull]
    public string UserId { get; }
    
    /// <summary>
    /// 获取当前用户的名称。
    /// </summary>
    [CanBeNull]
    public string UserName { get; }
}