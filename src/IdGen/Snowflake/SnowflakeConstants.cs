namespace IdGen.Snowflake;

internal static class SnowflakeConstants
{
    public const int NodeBitLength = 10;
    public static readonly int NodeMax = (int)Math.Pow(2, NodeBitLength) - 1;

    public const int SequenceBitLength = 12;
    public static readonly int SequenceMax = (int)Math.Pow(2, SequenceBitLength) - 1;
}