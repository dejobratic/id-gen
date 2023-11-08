namespace IdGen.TwitterSnowflake;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}