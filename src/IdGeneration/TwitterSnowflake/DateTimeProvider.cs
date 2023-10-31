namespace IdGeneration.TwitterSnowflake;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.UtcNow;
}