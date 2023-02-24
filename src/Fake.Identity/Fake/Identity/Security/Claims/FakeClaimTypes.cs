using System.Security.Claims;

namespace Fake.Identity.Security.Claims;

public static class FakeClaimTypes
{
    /// <summary>
    /// 用户id
    /// </summary>
    public static string UserId { get; set; } = ClaimTypes.NameIdentifier;
    
    /// <summary>
    /// 用户名
    /// </summary>
    public static string UserName { get; set; } = ClaimTypes.Name;
    
    /// <summary>
    /// 角色
    /// </summary>
    public static string Role { get; set; } = ClaimTypes.Role;

    /// <summary>
    /// 邮箱
    /// </summary>
    public static string Email { get; set; } = ClaimTypes.Email;
    
    
    /// <summary>
    /// 昵称
    /// </summary>
    public static string NickName { get; set; } = "nick_name";

}