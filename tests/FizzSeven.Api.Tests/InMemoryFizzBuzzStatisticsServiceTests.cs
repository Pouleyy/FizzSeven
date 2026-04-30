using FizzSeven.Api.Features.Statistics;
using FizzSeven.Core.FizzBuzz;

namespace FizzSeven.Api.Tests;

public sealed class InMemoryFizzBuzzStatisticsServiceTests
{
    [Fact]
    public async Task RecordRequestAsync_AggregatesConcurrentRequests()
    {
        var service = new FizzSeven.Api.Features.Statistics.InMemoryFizzBuzzStatisticsService();
        var winner = new FizzBuzzRequest(3, 5, 15, "fizz", "buzz");
        var runnerUp = new FizzBuzzRequest(2, 7, 20, "foo", "bar");

        var winnerTasks = Enumerable.Range(0, 100)
            .Select(_ => service.RecordRequestAsync(winner, CancellationToken.None));
        var runnerUpTasks = Enumerable.Range(0, 40)
            .Select(_ => service.RecordRequestAsync(runnerUp, CancellationToken.None));

        await Task.WhenAll(winnerTasks.Concat(runnerUpTasks));

        var mostUsedRequest = await service.GetMostFrequentRequestAsync(CancellationToken.None);

        Assert.NotNull(mostUsedRequest);
        Assert.Equal(winner.Int1, mostUsedRequest.Int1);
        Assert.Equal(winner.Int2, mostUsedRequest.Int2);
        Assert.Equal(winner.Limit, mostUsedRequest.Limit);
        Assert.Equal(winner.Str1, mostUsedRequest.Str1);
        Assert.Equal(winner.Str2, mostUsedRequest.Str2);
        Assert.Equal(100, mostUsedRequest.Hits);
    }
}
