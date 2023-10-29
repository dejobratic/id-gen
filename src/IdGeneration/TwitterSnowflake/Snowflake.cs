namespace IdGeneration.TwitterSnowflake;

public class Snowflake : IIdGenerator
{
    private static readonly object _lock = new();
    private static readonly DateTime _epochTimestamp = new(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private readonly int _node = 0;
    private long _lastEpochMillisecons = 0;
    private int _sequence = 0;

    private readonly IDateTimeProvider _dateTime;

    public Snowflake(
        SnowflakeOptions options,
        IDateTimeProvider dateTime)
    {
        _node = options.Node;
        _dateTime = dateTime;

        ThrowIfInvalid();
    }

    private void ThrowIfInvalid()
    {
        if (_node < 0 || _node > SnowflakeConstants.NodeMax)
            throw new ArgumentException(nameof(_node), $"Node must be between 0 and {SnowflakeConstants.NodeMax}.");
    }

    public long NewId()
    {
        lock (_lock)
        {
            _lastEpochMillisecons = ResolveEpochMilliseconds();
            return CreateId();
        }
    }

    private long ResolveEpochMilliseconds()
    {
        long epochMilliseconds = GetEpochMilliseconds();
        if (epochMilliseconds == _lastEpochMillisecons)
        {
            epochMilliseconds = IncrementMilliseconds(epochMilliseconds);
        }
        else
        {
            ResetSequence();
        }

        return epochMilliseconds;
    }

    private long GetEpochMilliseconds()
    {
        TimeSpan span = _dateTime.Now - _epochTimestamp;
        return (long)span.TotalMilliseconds;
    }

    private long IncrementMilliseconds(long epochMilliseconds)
    {
        IncrementSequence();

        if (IsSequenceOverflow())
        {
            ResetSequence();
            epochMilliseconds = WaitForNextEpochMillisecond(epochMilliseconds);
        }

        return epochMilliseconds;
    }

    private void IncrementSequence()
        => _sequence++;

    private bool IsSequenceOverflow()
        => _sequence > SnowflakeConstants.SequenceMax;

    private void ResetSequence()
        => _sequence = 0;

    private long WaitForNextEpochMillisecond(long epochMilliseconds)
    {
        while (epochMilliseconds == _lastEpochMillisecons)
            epochMilliseconds = GetEpochMilliseconds();

        return epochMilliseconds;
    }

    private long CreateId()
    {
        long id = _lastEpochMillisecons << SnowflakeConstants.NodeBitLength + SnowflakeConstants.SequenceBitLength;
        id |= (long)_node << SnowflakeConstants.SequenceBitLength;
        id |= (long)_sequence;

        return id;
    }
}