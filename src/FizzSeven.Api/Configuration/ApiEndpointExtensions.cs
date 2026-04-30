using FizzSeven.Api.Endpoints;
using Scalar.AspNetCore;

namespace FizzSeven.Api.Configuration;

public static class ApiEndpointExtensions
{
    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
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

        return app;
    }
}
