using IdGen.TwitterSnowflake;

namespace IdGen.Tests.Integration.TwitterSnowflake;

public class DateTimeProviderTests
{
    private readonly DateTimeProvider _sut = new();

    [Fact]
    public void Now_WhenInvoked_ReturnsUtcNow()
    {
        // Arrange
        var expected = DateTime.UtcNow;

        // Act
        var actual = _sut.Now;

        // Assert
        Assert.Equal(expected, actual, TimeSpan.FromMilliseconds(1));
    }
}