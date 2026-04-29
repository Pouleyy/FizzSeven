using System.Net;
using System.Net.Http.Json;
using FizzSeven.Api.Features.Statistics;
using FizzSeven.Core.FizzBuzz;
using Microsoft.AspNetCore.Mvc;

namespace FizzSeven.Api.Tests;

public sealed class FizzBuzzApiEndpointsTests
{
    [Fact]
    public async Task GetFizzBuzz_ReturnsExpectedSequence_AndTracksStatistics()
    {
        await using var factory = new ApiWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v1/fizzbuzz?int1=3&int2=5&limit=15&str1=fizz&str2=buzz");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<string[]>();

        Assert.NotNull(payload);
        Assert.Equal(
            ["1", "2", "fizz", "4", "buzz", "fizz", "7", "8", "fizz", "buzz", "11", "fizz", "13", "14", "fizzbuzz"],
            payload);
        Assert.Equal(new FizzBuzzRequest(3, 5, 15, "fizz", "buzz"), factory.StatisticsService.LastRecordedRequest);
    }

    [Fact]
    public async Task GetFizzBuzz_ReturnsValidationProblem_ForInvalidInput()
    {
        await using var factory = new ApiWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v1/fizzbuzz?int1=3&int2=5&limit=101&str1=&str2=buzz");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        Assert.NotNull(problem);
        Assert.Contains("Limit", problem.Errors.Keys);
        Assert.Contains("Str1", problem.Errors.Keys);
    }

    [Fact]
    public async Task GetStatistics_ReturnsMostUsedRequest()
    {
        var statisticsService = new InMemoryFizzBuzzStatisticsService
        {
            MostUsedRequest = new MostUsedFizzBuzzRequestResponse(3, 5, 100, "fizz", "buzz", 7)
        };

        await using var factory = new ApiWebApplicationFactory(statisticsService);
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v1/fizzbuzz/statistics");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<MostUsedFizzBuzzRequestResponse>();

        Assert.Equal(statisticsService.MostUsedRequest, payload);
    }

    [Fact]
    public async Task GetStatistics_ReturnsNotFound_WhenNoRequestWasRecorded()
    {
        await using var factory = new ApiWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v1/fizzbuzz/statistics");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal("No statistics available", problem.Title);
    }
}
