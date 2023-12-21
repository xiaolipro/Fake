namespace Fake.Identity.Users;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }

    /// <summary>
    /// 用户唯一标识
    /// </summary>

    public string? UserId { get; }

    /// <summary>
    /// 获取当前用户的名称。
    /// </summary>

    public string? UserName { get; }
}