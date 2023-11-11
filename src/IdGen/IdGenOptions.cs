using IdGen.Snowflake;

namespace IdGen;

public class IdGenOptions
{
    public Generator Generator { get; set; } = Generator.Snowflake;
    public SnowflakeOptions SnowflakeOptions { get; set; } = new();
}