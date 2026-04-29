using FizzSeven.Api.Features.Statistics;
using FizzSeven.Core.FizzBuzz;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FizzSeven.Api.Features.FizzBuzz;

public static class FizzBuzzEndpoints
{
    public static IEndpointRouteBuilder MapFizzBuzzEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/v1/fizzbuzz", HandleAsync)
            .WithName("GenerateFizzBuzz")
            .WithSummary("Generate a configurable FizzBuzz sequence.")
            .WithDescription("Returns the string sequence from 1 to limit using the provided replacement rules.")
            .Produces<IReadOnlyList<string>>(StatusCodes.Status200OK)
            .ProducesValidationProblem();

        return endpoints;
    }

    private static async Task<Results<Ok<IReadOnlyList<string>>, ValidationProblem>> HandleAsync(
        [AsParameters] FizzBuzzQuery query,
        FizzBuzzQueryValidator validator,
        IFizzBuzzSequenceService sequenceService,
        IFizzBuzzStatisticsService statisticsService,
        CancellationToken cancellationToken)
    {
        var errors = validator.Validate(query);

        if (errors.Count > 0)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var request = query.ToRequest();
        var response = sequenceService.Generate(request);

        await statisticsService.RecordRequestAsync(request, cancellationToken);

        return TypedResults.Ok(response);
    }
}
