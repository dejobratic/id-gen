namespace IdGen.Snowflake;

public class SnowflakeOptions
{
    public int Node { get; set; } = 0;
    public SnowflakeOverflowStrategy OverflowStrategy { get; set; } = SnowflakeOverflowStrategy.WaitForNextMillisecond;
}