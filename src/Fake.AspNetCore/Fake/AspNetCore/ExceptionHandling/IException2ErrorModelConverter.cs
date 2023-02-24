using System;
using Fake.AspNetCore.Http;

namespace Fake.AspNetCore.ExceptionHandling;

/// <summary>
/// 提供将 <see cref="Exception"/> 转化为 <see cref="RemoteServiceErrorModel"/> 的能力
/// </summary>
public interface IException2ErrorModelConverter
{
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    RemoteServiceErrorModel Convert(Exception exception, Action<FakeExceptionHandlingOptions> options = null);
}