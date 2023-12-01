using System;
using System.Runtime.Serialization;
using Fake.ExceptionHandling;
using Fake.Logging;
using Microsoft.Extensions.Logging;

namespace Fake.Authorization;

[Serializable]
public class FakeAuthorizationException : FakeException, IHasLogLevel, IHasErrorCode
{
    /// <summary>
    /// 异常等级，默认：LogLevel.Warning
    /// </summary>
    public LogLevel LogLevel { get; set; }

    /// <summary>
    /// 异常代码
    /// </summary>
    public string? Code { get; }

    public FakeAuthorizationException()
    {
        LogLevel = LogLevel.Warning;
    }

    public FakeAuthorizationException(string message)
        : base(message)
    {
        LogLevel = LogLevel.Warning;
    }

    public FakeAuthorizationException(string? message, Exception? innerException)
        : base(message, innerException)
    {
        LogLevel = LogLevel.Warning;
    }

    public FakeAuthorizationException(string? message = null, string? code = null, Exception? innerException = null)
        : base(message, innerException)
    {
        Code = code;
        LogLevel = LogLevel.Warning;
    }

    public FakeAuthorizationException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {
    }
}