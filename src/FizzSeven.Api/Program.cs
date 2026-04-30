using FizzSeven.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddFizzBuzzServices(builder.Configuration);
builder.Services.AddStatisticsServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
app.MapApiEndpoints();
app.MapDefaultEndpoints();

app.Run();

public partial class Program;
