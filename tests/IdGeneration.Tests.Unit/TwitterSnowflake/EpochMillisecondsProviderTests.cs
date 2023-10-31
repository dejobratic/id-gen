using IdGeneration.Tests.Unit.TwitterSnowflake.Fakes;
using IdGeneration.TwitterSnowflake;

namespace IdGeneration.Tests.Unit.TwitterSnowflake;

public class EpochMillisecondsProviderTests
{
    private readonly EpochMillisecondsProvider _sut;

    private readonly DateTimeFake _dateTimeFake = new();

    public EpochMillisecondsProviderTests()
    {
        _sut = new EpochMillisecondsProvider(_dateTimeFake);
    }

    [Fact]
    public void GetCurrentSinceEpoch_WhenInvoked_ReturnsMillisecondsSinceEpoch()
    {
        // Arrange
        _dateTimeFake.Setup(dateTime: new DateTime(2023, 1, 10, 0, 0, 0, DateTimeKind.Utc), occurences: 1);

        var epochTimestamp = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Act
        var actual = _sut.GetCurrentSinceEpoch(epochTimestamp);

        // Assert
        Assert.Equal(777600000, actual);
    }

    [Fact]
    public void GetNextSinceEpoch_WhenInvoked_ReturnsNextMillisecondsSinceEpoch()
    {
        // Arrange
        var dateTime = new DateTime(2023, 1, 10, 0, 0, 0, DateTimeKind.Utc);

        _dateTimeFake.Setup(dateTime, occurences: 10);
        _dateTimeFake.Setup(dateTime.AddMilliseconds(1), occurences: 1);

        var epochTimestamp = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var lastEpochMilliseconds = 777600000;

        // Act
        var actual = _sut.GetNextSinceEpoch(epochTimestamp, lastEpochMilliseconds);

        // Assert
        Assert.Equal(777600001, actual);
    }
}