using IdGen.Snowflake;

namespace IdGen.Tests.Integration.Snowflake
{
    public class EpochMillisecondsProviderTests
    {
        private readonly EpochMillisecondsProvider _sut = new(new DateTimeProvider());

        [Fact]
        public void GetCurrentSinceEpoch_WhenInvoked_ReturnsMillisecondsSinceEpoch()
        {
            // Arrange
            var epochTimestamp = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expected = (long)(DateTime.UtcNow - epochTimestamp).TotalMilliseconds;

            // Act
            var actual = _sut.GetCurrentSinceEpoch(epochTimestamp);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNextSinceEpoch_WhenInvoked_ReturnsNextMillisecondsSinceEpoch()
        {
            // Arrange
            var epochTimestamp = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var lastEpochMilliseconds = (long)(DateTime.UtcNow - epochTimestamp).TotalMilliseconds;

            // Act
            var actual = _sut.GetNextSinceEpoch(epochTimestamp, lastEpochMilliseconds);

            // Assert
            Assert.Equal(lastEpochMilliseconds + 1, actual);
        }
    }
}