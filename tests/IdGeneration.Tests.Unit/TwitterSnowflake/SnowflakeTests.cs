using System.Collections.Concurrent;
using IdGeneration.TwitterSnowflake;

namespace IdGeneration.Tests.Unit.TwitterSnowflake;

public class SnowflakeTests
{
    private readonly Snowflake _sut;

    private readonly SnowflakeOptions _options;
    private readonly DateTimeFake _dateTime;

    public SnowflakeTests()
    {
        _options = new()
        {
            Node = Random.Shared.Next(0, 1023)
        };
        _dateTime = new DateTimeFake();

        _sut = new Snowflake(_options, _dateTime);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1024)]
    public void Constructor_WhenNodeIsInvalid_ThrowsArgumentException(int node)
    {
        // Arrange
        var options = new SnowflakeOptions { Node = node };

        // Act
        Snowflake action() => new(options, _dateTime);

        // Assert
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("_node (Parameter 'Node must be between 0 and 1023.')", exception.Message);
    }

    [Fact]
    public void NewId_WhenInvoked_ReturnsId()
    {
        // Arrange
        // Act
        long actual = _sut.NewId();

        // Assert
        Assert.True(actual > 0);
    }

    [Fact]
    public void NewId_WhenInvokedSequentially_ReturnsDistinctIdsInAscendingOrder()
    {
        // Arrange
        var idCount = 100000;
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
        var idCount = 100000;
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

    [Fact]
    public void NewId_WhenInvokedUpToSequenceMaxTimesInSameMillisecond_IncrementsSequence()
    {
        // Arrange
        _dateTime.Returns = new DateTime(2023, 1, 10, 0, 0, 0, DateTimeKind.Utc);

        var idCount = 4096;
        var ids = new long[idCount];

        // Act
        for (int i = 0; i < idCount; i++)
        {
            ids[i] = _sut.NewId();
        }

        // Assert
        Assert.Equal(idCount, ids.Length);
        Assert.Equal(((long)777600000 << 22) | (uint)_options.Node << 12, ids[0]);
        Assert.Equal(((long)777600000 << 22) | (uint)_options.Node << 12 | 4095, ids[^1]);
        Assert.True(ids.SequenceEqual(ids.OrderBy(x => x)));
    }
}