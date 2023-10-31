namespace IdGeneration.TwitterSnowflake;

public class Snowflake : IIdGenerator
{
    private static readonly object _lock = new();
    private static readonly DateTime _epochTimestamp = new(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private readonly int _node = 0;
    private long _lastEpochMillisecons = 0;
    private int _sequence = 0;

    private readonly IEpochMillisecondsProvider _epochMilliseconds;

    public Snowflake(
        SnowflakeOptions options,
        IEpochMillisecondsProvider epochMilliseconds)
    {
        _node = options.Node;
        _epochMilliseconds = epochMilliseconds;

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
        var epochMilliseconds = _epochMilliseconds.GetCurrentSinceEpoch(_epochTimestamp);
        if (IsOnLastEpochMilliseconds(epochMilliseconds))
        {
            IncrementSequence();
            epochMilliseconds = IncrementEpochMillisecondsOnSequenceOverflow(epochMilliseconds);
        }
        else
        {
            ResetSequence();
        }

        return epochMilliseconds;
    }

    private bool IsOnLastEpochMilliseconds(long epochMilliseconds)
        => epochMilliseconds == _lastEpochMillisecons;

    private long IncrementEpochMillisecondsOnSequenceOverflow(long epochMilliseconds)
    {
        if (IsSequenceOverflow())
        {
            ResetSequence();
            epochMilliseconds = _epochMilliseconds.GetNextSinceEpoch(_epochTimestamp, epochMilliseconds);
        }

        return epochMilliseconds;
    }

    private void IncrementSequence()
        => _sequence++;

    private bool IsSequenceOverflow()
        => _sequence > SnowflakeConstants.SequenceMax;

    private void ResetSequence()
        => _sequence = 0;

    private long CreateId()
    {
        long id = _lastEpochMillisecons << SnowflakeConstants.NodeBitLength + SnowflakeConstants.SequenceBitLength;
        id |= (long)_node << SnowflakeConstants.SequenceBitLength;
        id |= (long)_sequence;

        return id;
    }
}