using System.Security.Cryptography;

namespace Fake.IdGenerators.GuidGenerator;

/// <summary>
/// 有序guid生成器
/// take from https://github.com/jhtodd/SequentialGuid/blob/master/SequentialGuid/Classes/SequentialGuid.cs
/// </summary>
public class SequentialGuidGenerator(SequentialGuidType sequentialGuidType) : GuidGeneratorBase
{
    private static readonly RandomNumberGenerator RandomNumberGenerator = RandomNumberGenerator.Create();

    /// <summary>
    /// 生成有序guid
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public override Guid Generate()
    {
        // 使用10个随机字节，80位的随机组合已经足够满足唯一性了
        var randomBytes = GenerateRandomBytes();

        // 使用剩下6个字节放时间戳（ms级别）做有序性，1Ticks=100ns，除以10,000得到的刚好是ms
        var timestampBytes = GenerateTimestampBytes();

        // 1个Guid由16个字节组成
        return GenerateGuid(timestampBytes, randomBytes);
    }

    private Guid GenerateGuid(byte[] timestampBytes, byte[] randomBytes)
    {
        var guidBytes = new byte[16];

        switch (sequentialGuidType)
        {
            case SequentialGuidType.SequentialAsString:
                // 6字节时间戳+10字节随机数
                Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                // 如果格式化为string，我们必须补偿这样一个事实：.NET将Data1和Data2块分别视为Int32和Int16。
                // 这意味着它在小端系统上切换顺序。所以，我们必须再次逆转。
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(guidBytes, 0, 4);
                    Array.Reverse(guidBytes, 4, 2);
                }

                break;
            case SequentialGuidType.SequentialAsBinaryAtStart:
                // 6字节时间戳+10字节随机数
                Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);
                break;
            case SequentialGuidType.SequentialAsBinaryAtEnd:
                // 10字节随机数+6字节时间戳
                Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                break;
            default:
                throw new NotSupportedException($"不支持的有序Guid类型 {sequentialGuidType}");
        }

        return new Guid(guidBytes);
    }

    private byte[] GenerateTimestampBytes()
    {
        // long占8个字节，我们抛弃前两个，使用6个字节48位存放毫秒，已经足够我们使用到公元8900年了
        long timestamp = DateTime.UtcNow.Ticks / 10000L;
        var timestampBytes = BitConverter.GetBytes(timestamp);

        // 小端鸡：最低有效位保存在内存地址的最低位置
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(timestampBytes);
        }

        return timestampBytes;
    }

    private byte[] GenerateRandomBytes()
    {
        var randomBytes = new byte[10];

        // 这里使用RandomNumberGenerator生成随机，而不是Guid.NewGuid().ToByteArray()。
        // 牺牲了一定性能，但RNG产生的数字似乎是真正随机的，因为它们使用互不相关的真随机种子和外部环境因素
        // （如鼠标移动、键盘敲击等中断数据）来生成伪随机数。
        RandomNumberGenerator.GetBytes(randomBytes);

        return randomBytes;
    }
}