namespace IdGen.TwitterSnowflake;

public class EpochMillisecondsProvider : IEpochMillisecondsProvider
{
    private readonly IDateTimeProvider _dateTime;

    public EpochMillisecondsProvider(IDateTimeProvider dateTime)
    {
        _dateTime = dateTime;
    }

    public long GetCurrentSinceEpoch(DateTime epochTimestamp)
    {
        TimeSpan span = _dateTime.Now - epochTimestamp;
        return (long)span.TotalMilliseconds;
    }

    public long GetNextSinceEpoch(DateTime epochTimestamp, long lastEpochMilliseconds)
    {
        var epochMilliseconds = GetCurrentSinceEpoch(epochTimestamp);

        while (epochMilliseconds == lastEpochMilliseconds)
            epochMilliseconds = GetCurrentSinceEpoch(epochTimestamp);

        return epochMilliseconds;
    }
}