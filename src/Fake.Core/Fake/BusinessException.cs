using System.Runtime.Serialization;
using Fake.Logging;
using Microsoft.Extensions.Logging;

namespace Fake;

[Serializable]
public class BusinessException : FakeException, IHasLogLevel
{
    public LogLevel LogLevel { get; set; }

    /// <summary>
    /// 用于序列化的构造函数
    /// </summary>
    public BusinessException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {
    }

    public BusinessException(
        string? message = null,
        Exception? innerException = null,
        LogLevel logLevel = LogLevel.Warning)
        : base(message, innerException)
    {
        LogLevel = logLevel;
    }
}