namespace IdGen.TwitterSnowflake;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.UtcNow;
}