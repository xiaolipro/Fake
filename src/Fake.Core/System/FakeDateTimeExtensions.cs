using Fake.Timing;

namespace System;

public static class FakeDateTimeExtensions
{
    /// <summary>
    /// 转时间戳
    /// </summary>
    /// <param name="dateTimeOffset"></param>
    /// <param name="timestampType">Timestamp type: milliseconds: 1, seconds: 2</param>
    /// <returns></returns>
    public static long ToTimestamp(this DateTimeOffset dateTimeOffset, TimestampType timestampType)
    {
        if (timestampType == TimestampType.Milliseconds)
            return dateTimeOffset.ToUnixTimeMilliseconds();

        return dateTimeOffset.ToUnixTimeSeconds();
    }
}