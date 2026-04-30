using FizzSeven.Api.Features.Statistics;
using StackExchange.Redis;

namespace FizzSeven.Api.Configuration;

public static class StatisticsConfigurationExtensions
{
    public static IServiceCollection AddStatisticsServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<StatisticsOptions>()
            .Bind(configuration.GetSection(StatisticsOptions.SectionName))
            .ValidateOnStart();

        var statisticsOptions = configuration.GetSection(StatisticsOptions.SectionName).Get<StatisticsOptions>() ?? new StatisticsOptions();

        if (statisticsOptions.UseInMemoryStore)
        {
            services.AddSingleton<IFizzBuzzStatisticsService, InMemoryFizzBuzzStatisticsService>();
            return services;
        }

        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            var connectionString = configuration.GetConnectionString("cache")
                ?? throw new InvalidOperationException("Redis connection string 'cache' is not configured. Launch the API through Aspire AppHost or enable Statistics:UseInMemoryStore.");

            var redisConfiguration = ConfigurationOptions.Parse(connectionString, true);
            redisConfiguration.AbortOnConnectFail = false;
            redisConfiguration.ClientName = "FizzSeven.Api";

            return ConnectionMultiplexer.Connect(redisConfiguration);
        });
        services.AddSingleton<IFizzBuzzStatisticsService, RedisFizzBuzzStatisticsService>();

        return services;
    }
}
