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

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "FizzSeven API";
    });
}

app.MapGet("/", () => Results.Redirect("/scalar/v1"))
    .ExcludeFromDescription();

app.MapFizzBuzzEndpoints();
app.MapDefaultEndpoints();

app.Run();

public partial class Program;
