using Fake.Timing;

namespace Fake.IdGenerators.Snowflake;

public class SnowflakeIdGeneratorOptions
{
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; } = new(2022, 1, 1);

    /// <summary>
    /// 时间戳类型
    /// tips: ms精度41位可用34年
    /// </summary>
    public TimestampType TimestampType { get; set; } = TimestampType.Milliseconds;

    /// <summary>
    /// 序列数占字节
    /// </summary>
    public int SequenceBits { get; set; } = 12;

    /// <summary>
    /// 机器Id数占字节
    /// </summary>
    public int WorkerIdBits { get; set; } = 10;

    public int MaxWorkerId => (1 << WorkerIdBits) - 1;
}