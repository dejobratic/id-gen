namespace IdGen.Snowflake;

public enum SnowflakeOverflowStrategy
{
    WaitForNextMillisecond = 1,
    ThrowException,
}