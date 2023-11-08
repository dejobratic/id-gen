using IdGen.TwitterSnowflake;

namespace IdGen.Tests.Unit.TwitterSnowflake.Fakes;

public class DateTimeFake : IDateTimeProvider
{
    private readonly Queue<(DateTime, int)> _dateTimeByOccurence = new();

    private int _occurences = 0;

    public void Setup(DateTime dateTime, int occurences)
        => _dateTimeByOccurence.Enqueue((dateTime, occurences));

    public DateTime? Returns { get; set; }

    public DateTime Now
    {
        get
        {
            var (dateTime, occurences) = _dateTimeByOccurence.Peek();

            if (_occurences == occurences - 1)
            {
                _dateTimeByOccurence.Dequeue();
                _occurences = 0;
            }
            else
            {
                _occurences++;
            }

            return dateTime;
        }
    }
}