namespace IdGen.TwitterSnowflake;

public interface IEpochMillisecondsProvider
{
    long GetCurrentSinceEpoch(DateTime epochTimestamp);

    long GetNextSinceEpoch(DateTime epochTimestamp, long lastEpochMilliseconds);
}