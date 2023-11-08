using IdGen.TwitterSnowflake;
using Microsoft.Extensions.DependencyInjection;

namespace IdGen;

public static class DependencyInjection
{
    public static IServiceCollection AddIdGen(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEpochMillisecondsProvider, EpochMillisecondsProvider>();
        services.AddSingleton(new SnowflakeOptions { Node = 0 });
        services.AddSingleton<IIdGenerator, Snowflake>();

        return services;
    }
}