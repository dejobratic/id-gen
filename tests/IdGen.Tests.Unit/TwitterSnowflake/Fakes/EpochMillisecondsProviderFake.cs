using IdGen.Snowflake;

namespace IdGen.Tests.Unit.Snowflake.Fakes;

public class EpochMillisecondsProviderFake : IEpochMillisecondsProvider
{
    private readonly Queue<(long, int)> _millisecondsByOccurence = new();

    private int _occurences = 0;

    public void Setup(long epochMilliseconds, int occurences)
        => _millisecondsByOccurence.Enqueue((epochMilliseconds, occurences));

    public long GetCurrentSinceEpoch(DateTime epochTimestamp)
    {
        var (millisecond, occurences) = _millisecondsByOccurence.Peek();

        if (_occurences == occurences - 1)
        {
            _millisecondsByOccurence.Dequeue();
            _occurences = 0;
        }
        else
        {
            _occurences++;
        }

        return millisecond;
    }

    public long GetNextSinceEpoch(DateTime epochTimestamp, long lastEpochMilliseconds)
    {
        var epochMilliseconds = GetCurrentSinceEpoch(epochTimestamp);

        while (epochMilliseconds == lastEpochMilliseconds)
            epochMilliseconds = GetCurrentSinceEpoch(epochTimestamp);

        return epochMilliseconds;
    }
}