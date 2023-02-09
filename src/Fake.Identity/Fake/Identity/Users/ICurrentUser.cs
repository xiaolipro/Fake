namespace Fake.Identity.Users;

public interface ICurrentUser<TKey>
{
    /// <summary>
    /// 用户唯一标识
    /// </summary>
    public TKey UserId { get; }
    
    bool IsAuthenticated { get; }
    
    string UserName { get; }
}