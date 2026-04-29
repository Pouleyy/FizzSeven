using FizzSeven.Api.Features.Statistics;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace FizzSeven.Api.Tests;

public sealed class ApiWebApplicationFactory(InMemoryFizzBuzzStatisticsService? statisticsService = null) : WebApplicationFactory<Program>
{
    public InMemoryFizzBuzzStatisticsService StatisticsService { get; } = statisticsService ?? new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IFizzBuzzStatisticsService>();
            services.RemoveAll<IConnectionMultiplexer>();
            services.AddSingleton<IFizzBuzzStatisticsService>(StatisticsService);
        });
    }
}
