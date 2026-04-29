using FizzSeven.Api.Features.Statistics;
using FizzSeven.Core.FizzBuzz;

namespace FizzSeven.Api.Tests;

public sealed class InMemoryFizzBuzzStatisticsService : IFizzBuzzStatisticsService
{
    public FizzBuzzRequest? LastRecordedRequest { get; private set; }

    public MostUsedFizzBuzzRequestResponse? MostUsedRequest { get; set; }

    public Task RecordRequestAsync(FizzBuzzRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LastRecordedRequest = request;

        return Task.CompletedTask;
    }

    public Task<MostUsedFizzBuzzRequestResponse?> GetMostFrequentRequestAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(MostUsedRequest);
    }
}
