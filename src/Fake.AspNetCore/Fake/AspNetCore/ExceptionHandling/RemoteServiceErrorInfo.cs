namespace Fake.AspNetCore.ExceptionHandling;

/// <summary>
/// 远程服务异常模型
/// </summary>
[Serializable]
public class RemoteServiceErrorInfo
{
    /// <summary>
    /// 异常代码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 异常信息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 异常栈
    /// </summary>
    public string? StackTrace { get; set; }

    public RemoteServiceErrorInfo()
    {
    }

    public RemoteServiceErrorInfo(string code, string? message = null)
    {
        Code = code;
        Message = message;
    }
}