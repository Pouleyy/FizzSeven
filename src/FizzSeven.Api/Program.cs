using FizzSeven.Api.Configuration;
using FizzSeven.Api.Endpoints;
using FizzSeven.Api.Features.FizzBuzz;
using FizzSeven.Api.Features.Statistics;
using FizzSeven.Core.FizzBuzz;
using Scalar.AspNetCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddOptions<FizzBuzzOptions>()
    .Bind(builder.Configuration.GetSection(FizzBuzzOptions.SectionName))
    .ValidateDataAnnotations()
    .Validate(
        options => options.MaxLimit > 0,
        $"{FizzBuzzOptions.SectionName}:{nameof(FizzBuzzOptions.MaxLimit)} must be greater than zero.")
    .ValidateOnStart();
builder.Services.AddOptions<StatisticsOptions>()
    .Bind(builder.Configuration.GetSection(StatisticsOptions.SectionName))
    .ValidateOnStart();

builder.Services.AddSingleton<IFizzBuzzSequenceService, FizzBuzzSequenceService>();
builder.Services.AddScoped<FizzBuzzQueryValidator>();
var statisticsOptions = builder.Configuration.GetSection(StatisticsOptions.SectionName).Get<StatisticsOptions>() ?? new StatisticsOptions();

if (statisticsOptions.UseInMemoryStore)
{
    builder.Services.AddSingleton<IFizzBuzzStatisticsService, InMemoryFizzBuzzStatisticsService>();
}
else
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    {
        var connectionString = builder.Configuration.GetConnectionString("cache")
            ?? throw new InvalidOperationException("Redis connection string 'cache' is not configured. Launch the API through Aspire AppHost or enable Statistics:UseInMemoryStore.");

        var configuration = ConfigurationOptions.Parse(connectionString, true);
        configuration.AbortOnConnectFail = false;
        configuration.ClientName = "FizzSeven.Api";

        return ConnectionMultiplexer.Connect(configuration);
    });
    builder.Services.AddSingleton<IFizzBuzzStatisticsService, RedisFizzBuzzStatisticsService>();
}

var app = builder.Build();

app.UseExceptionHandler();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options.Title = "FizzSeven API";
    });
}

app.MapGet("/", () => TypedResults.Ok("FizzSeven API is running."))
    .ExcludeFromDescription();

app.MapFizzBuzzEndpoints();
app.MapStatisticsEndpoints();
app.MapDefaultEndpoints();

app.Run();

public partial class Program;
