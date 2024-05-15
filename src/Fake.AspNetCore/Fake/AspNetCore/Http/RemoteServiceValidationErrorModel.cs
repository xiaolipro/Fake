namespace Fake.AspNetCore.Http;

[Serializable]
public class RemoteServiceValidationErrorModel
{
    /// <summary>
    /// 验证错误消息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 相关成员 (fields/properties).
    /// </summary>
    public string[] Members { get; set; } = [];

    public RemoteServiceValidationErrorModel()
    {
    }

    public RemoteServiceValidationErrorModel(string message)
    {
        Message = message;
    }

    public RemoteServiceValidationErrorModel(string message, params string[] members)
        : this(message)
    {
        Members = members;
    }
}