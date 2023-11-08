using IdGen.TwitterSnowflake;
using System.Collections.Concurrent;

namespace IdGen.Tests.Integration.TwitterSnowflake;

public class SnowflakeTests
{
    private readonly Snowflake _sut;

    public SnowflakeTests()
    {
        var options = new SnowflakeOptions()
        {
            Node = Random.Shared.Next(0, 1024)
        };
        var dateTime = new DateTimeProvider();
        var epochMilliseconds = new EpochMillisecondsProvider(dateTime);

        _sut = new Snowflake(options, epochMilliseconds);
    }

    [Fact]
    public void NewId_WhenInvoked_ReturnsId()
    {
        // Arrange Act
        long actual = _sut.NewId();

        // Assert
        Assert.True(actual > 0);
    }

    [Fact]
    public void NewId_WhenInvokedSequentially_ReturnsDistinctIdsInAscendingOrder()
    {
        // Arrange
        var idCount = 1000000;
        var ids = new long[idCount];

        // Act
        for (int i = 0; i < idCount; i++)
        {
            ids[i] = _sut.NewId();
        }

        // Assert
        Assert.Equal(idCount, ids.Length);
        Assert.Equal(idCount, ids.Distinct().Count());
        Assert.True(ids.SequenceEqual(ids.Distinct()));
        Assert.True(ids.SequenceEqual(ids.OrderBy(x => x)));
    }

    [Fact]
    public void NewId_WhenInvokedInParallel_ReturnsDistinctIds()
    {
        // Arrange
        var idCount = 1000000;
        var ids = new ConcurrentBag<long>();

        // Act
        Parallel.For(0, idCount, _ =>
        {
            ids.Add(_sut.NewId());
        });

        // Assert
        Assert.Equal(idCount, ids.Count);
        Assert.Equal(idCount, ids.Distinct().Count());
        Assert.True(ids.SequenceEqual(ids.Distinct()));
    }
}