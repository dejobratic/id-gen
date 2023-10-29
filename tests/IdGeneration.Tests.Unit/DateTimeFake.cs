namespace IdGeneration.Tests.Unit;

public class DateTimeFake : IDateTimeProvider
{
    public DateTime? Returns { get; set; }

    public DateTime Now => Returns is null ? DateTime.UtcNow : Returns.Value;
}
