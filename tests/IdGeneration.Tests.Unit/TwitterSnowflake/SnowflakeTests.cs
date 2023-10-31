using IdGeneration.Tests.Unit.TwitterSnowflake.Fakes;
using IdGeneration.TwitterSnowflake;

namespace IdGeneration.Tests.Unit.TwitterSnowflake;

public class SnowflakeTests
{
    private readonly Snowflake _sut;

    private readonly SnowflakeOptions _options;
    private readonly EpochMillisecondsProviderFake _epochMilliseconds;

    public SnowflakeTests()
    {
        _options = new()
        {
            Node = Random.Shared.Next(0, 1024)
        };
        _epochMilliseconds = new EpochMillisecondsProviderFake();

        _sut = new Snowflake(_options, _epochMilliseconds);
    }

    [Fact]
    public void Constructor_WhenNodeLessThanAllowed_ThrowsArgumentException()
    {
        // Arrange
        var options = new SnowflakeOptions { Node = Random.Shared.Next(int.MinValue, 0) };

        // Act
        Snowflake action() => new(options, _epochMilliseconds);

        // Assert
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("_node (Parameter 'Node must be between 0 and 1023.')", exception.Message);
    }

    [Fact]
    public void Constructor_WhenNodeIsGreaterThanAllowed_ThrowsArgumentException()
    {
        // Arrange
        var options = new SnowflakeOptions { Node = Random.Shared.Next(1024, int.MaxValue) };

        // Act
        Snowflake action() => new(options, _epochMilliseconds);

        // Assert
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("_node (Parameter 'Node must be between 0 and 1023.')", exception.Message);
    }

    [Fact]
    public void NewId_WhenInvoked_ReturnsId()
    {
        // Arrange
        _epochMilliseconds.Setup(epochMilliseconds: 777600000, occurences: 4096);

        // Act
        long actual = _sut.NewId();

        // Assert
        Assert.True(actual > 0);
    }

    [Fact]
    public void NewId_WhenInvokedUpToSequenceMaxTimesInSameMillisecond_IncrementsSequence()
    {
        // Arrange
        _epochMilliseconds.Setup(epochMilliseconds: 777600000, occurences: 4096);

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

    [Fact]
    public void NewId_WhenInvokedOerSequenceMaxTimesInSameMillisecond_ResetsSequenceAndIncrementsMilliseconds()
    {
        // Arrange
        _epochMilliseconds.Setup(epochMilliseconds: 777600000, occurences: 4999);
        _epochMilliseconds.Setup(epochMilliseconds: 777600001, occurences: 1);

        var idCount = 4097;
        var ids = new long[idCount];

        // Act
        for (int i = 0; i < idCount; i++)
        {
            ids[i] = _sut.NewId();
        }

        // Assert
        Assert.Equal(idCount, ids.Length);
        Assert.Equal(((long)777600000 << 22) | (uint)_options.Node << 12, ids[0]);
        Assert.Equal(((long)777600000 << 22) | (uint)_options.Node << 12 | 4095, ids[^2]);
        Assert.Equal(((long)777600001 << 22) | (uint)_options.Node << 12, ids[^1]);
        Assert.True(ids.SequenceEqual(ids.OrderBy(x => x)));
    }
}