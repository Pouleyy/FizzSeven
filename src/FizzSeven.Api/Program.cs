using FizzSeven.Api.Configuration;
using FizzSeven.Api.Endpoints;
using FizzSeven.Api.Features.FizzBuzz;
using FizzSeven.Core.FizzBuzz;
using Scalar.AspNetCore;

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

builder.Services.AddSingleton<IFizzBuzzSequenceService, FizzBuzzSequenceService>();
builder.Services.AddScoped<FizzBuzzQueryValidator>();
builder.Services.AddStatisticsServices(builder.Configuration);

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
