namespace Fake.Application;

/// <summary>
/// 应用服务异常模型
/// </summary>
[Serializable]
public class ApplicationServiceErrorInfo
{
    /// <summary>
    /// 异常信息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 异常明细
    /// </summary>
    public string? Details { get; set; }

    public ApplicationServiceErrorInfo()
    {
    }

    public ApplicationServiceErrorInfo(string? message = null)
    {
        Message = message;
    }
}