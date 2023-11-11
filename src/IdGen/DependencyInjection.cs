using IdGen.Snowflake;
using Microsoft.Extensions.DependencyInjection;

namespace IdGen;

public static class DependencyInjection
{
    public static IServiceCollection AddIdGen(this IServiceCollection services, Action<IdGenOptions>? configure = null)
    {
        var options = new IdGenOptions();
        if (configure is not null) configure(options);

        switch (options.Generator)
        {
            case Generator.Snowflake:
                services.ConfigureSnowflake(options.SnowflakeOptions);
                break;

            default:
                throw new Exception();
        }

        return services;
    }

    private static void ConfigureSnowflake(this IServiceCollection services, SnowflakeOptions options)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IEpochMillisecondsProvider, EpochMillisecondsProvider>();
        services.AddSingleton(options);
        services.AddSingleton<IIdGenerator, Snowflake.Snowflake>();
    }
}