namespace IdGen.Snowflake;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}