using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FizzSeven.Api.Features.Statistics;

public static class StatisticsEndpoints
{
    public static IEndpointRouteBuilder MapStatisticsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/v1/fizzbuzz/statistics", HandleAsync)
            .WithName("GetFizzBuzzStatistics")
            .WithSummary("Get the most frequent FizzBuzz request.")
            .WithDescription("Returns the most frequently used FizzBuzz parameter set and its number of hits.")
            .Produces<MostUsedFizzBuzzRequestResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        return endpoints;
    }

    private static async Task<Results<Ok<MostUsedFizzBuzzRequestResponse>, NotFound<ProblemDetails>>> HandleAsync(
        IFizzBuzzStatisticsService statisticsService,
        CancellationToken cancellationToken)
    {
        var statistics = await statisticsService.GetMostFrequentRequestAsync(cancellationToken);

        if (statistics is null)
        {
            return TypedResults.NotFound(new ProblemDetails
            {
                Title = "No statistics available",
                Detail = "No successful FizzBuzz request has been recorded yet.",
                Status = StatusCodes.Status404NotFound
            });
        }

        return TypedResults.Ok(statistics);
    }
}
