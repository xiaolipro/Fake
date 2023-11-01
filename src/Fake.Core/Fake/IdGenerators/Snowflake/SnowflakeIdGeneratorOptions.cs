using Fake.Timing;

namespace Fake.IdGenerators.Snowflake;

public class SnowflakeIdGeneratorOptions
{
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 时间戳类型
    /// </summary>
    public TimestampType TimestampType { get; set; }

    /// <summary>
    /// 序列数占字节
    /// </summary>
    public int SequenceBits { get; set; }

    /// <summary>
    /// 机器Id数占字节
    /// </summary>
    public int WorkerIdBits { get; set; }

    public int MaxWorkerId => (1 << WorkerIdBits) - 1;
}