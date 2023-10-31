namespace IdGeneration.TwitterSnowflake;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}