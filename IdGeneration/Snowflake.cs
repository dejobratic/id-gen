using System;

namespace IdGeneration
{
    public class Snowflake
    {
        private const int ID_BIT_LENGTH = 64;
        private const int TIMESTAMP_BIT_LENGTH = 41;
        private const int NODE_BIT_LENGTH = 10;
        private const int SEQUENCE_BIT_LENGTH = 12;

        private static readonly object _lock = new object();
        private static readonly DateTime _epochTimestamp = new DateTime(2021, 1, 1);

        private static readonly int _maxNode = (int)Math.Pow(2, NODE_BIT_LENGTH);
        private static readonly int _maxSequence = (int)Math.Pow(2, SEQUENCE_BIT_LENGTH);
        
        private static long _lastEpochMillisecons;
        private static int _node;
        private static int _sequence;

        public static long NewId()
        {
            lock (_lock)
            {
                long currentEpochMilliseconds = GetCurrentEpochMilliseconds();
                if (currentEpochMilliseconds == _lastEpochMillisecons)
                {
                    IncrementSequence();
                    currentEpochMilliseconds = IncrementMillisecondsOnSequenceOverflow(currentEpochMilliseconds);
                }
                else
                {
                    ResetSequence();
                }

                _lastEpochMillisecons = currentEpochMilliseconds;
                return CreateId(currentEpochMilliseconds);
            }
        }

        private static long GetCurrentEpochMilliseconds()
        {
            TimeSpan span = DateTime.UtcNow - _epochTimestamp;
            return (long)span.TotalMilliseconds;
        }

        private static void IncrementSequence()
            => _sequence++;

        private static long IncrementMillisecondsOnSequenceOverflow(long currentEpochMilliseconds)
        {
            if (_sequence > _maxSequence)
            {
                ResetSequence();
                currentEpochMilliseconds = WaitForNextMillisecond(currentEpochMilliseconds);
            }

            return currentEpochMilliseconds;
        }

        private static void ResetSequence()
            => _sequence = 0;

        private static long WaitForNextMillisecond(long currentEpochMilliseconds)
        {
            while (currentEpochMilliseconds == _lastEpochMillisecons)
                currentEpochMilliseconds = GetCurrentEpochMilliseconds();

            return currentEpochMilliseconds;
        }

        private static long CreateId(long currentEpochMilliseconds)
        {
            long id = currentEpochMilliseconds << (ID_BIT_LENGTH - TIMESTAMP_BIT_LENGTH);
            id |= (long)_node << NODE_BIT_LENGTH;
            id |= (long)_sequence;

            return id;
        }
    }
}
